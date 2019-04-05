<#
.Synopsis
    Copies the contents of a Power BI workspace to another Power BI workspace.
.Description
    Copies the contents of a Power BI workspace to another Power BI workspace, including dashboards, reports and datasets.
	This script creates the target workspace if it does not exist.
    This script uses the Power BI Management module for Windows PowerShell. If this module isn't installed, install it by using the command 'Install-Module -Name MicrosoftPowerBIMgmt -Scope CurrentUser'.
.Parameter SourceWorkspaceName
    The name of the workspace you'd like to copy the contents from.
.Parameter TargetWorkspaceName
    The name of the workspace you'd like to copy to. You must have edit access to the specified workspace.
.Parameter CreateTargetWorkspaceIfNotExists
    A flag to indicate if the script should create the target workspace if it doesn't exist. The default is to create the target workspace.
.Example
    PS C:\> .\CopyWorkspaceSampleScript.ps1 -SourceWorkspaceName "My Workspace" -TargetWorkspaceName "Copy of My Workspace"
	Copies the contents of the current user's personal workspace to a new workspace called "Copy of My Workspace".
#>

[CmdletBinding()]
param
(
    [string] $SourceWorkspaceName,

    [string] $TargetWorkspaceName,

    [bool] $CreateTargetWorkspaceIfNotExists = $true
)

#region Helper Functions 

function Assert-ModuleExists([string]$ModuleName) {
    $module = Get-Module $ModuleName -ListAvailable -ErrorAction SilentlyContinue
    if(!$module) {
        Write-Host "Installing module $ModuleName ..."
        Install-Module -Name $ModuleName -Force -Scope CurrentUser
        Write-Host "Module installed"
    }
    elseif($module.Version -ne '1.0.0' -and $module.Version -le '1.0.410') {
        Write-Host "Updating module $ModuleName ..."
        Update-Module -Name $ModuleName -Force -ErrorAction Stop
        Write-Host "Module updated"
    }
}

#endregion

# ==================================================================
# PART 1: Verify that the Power BI Management module is installed
#         and authenticate the current user.
# ==================================================================
Assert-ModuleExists -ModuleName "MicrosoftPowerBIMgmt"
Login-PowerBI

# ==================================================================
# PART 2: Getting source and target workspace
# ==================================================================
# STEP 2.1: Get the source workspace
$source_workspace_ID = ""
while (!$source_workspace_ID) {
	$source_workspace_name = if(-not($SourceWorkspaceName)) {
		 Read-Host -Prompt "Enter the name of the workspace you'd like to copy from" } else {
		 $SourceWorkspaceName }

    if($source_workspace_name -eq "My Workspace") {
        $source_workspace_ID = "me"
        break
    }

    $workspace = Get-PowerBIWorkspace -Name $source_workspace_name -ErrorAction SilentlyContinue

    if(!$workspace) {
        Write-Warning "Could not get a workspace with that name. Please try again, making sure to type the exact name of the workspace"  
    } else {
		$source_workspace_ID = $workspace.id
    }
}

# STEP 2.2: Get the target workspace
$target_workspace_ID = "" 
while (!$target_workspace_ID) {
    $target_workspace_name = if(-not($TargetWorkspaceName)) {
		 Read-Host -Prompt "Enter the name of the workspace you'd like to copy to" } else {
		 $TargetWorkspaceName }
	
    $target_workspace = Get-PowerBIWorkspace -Name $target_workspace_name -ErrorAction SilentlyContinue

    if(!$target_workspace -and $CreateTargetWorkspaceIfNotExists -eq $true) {
        $target_workspace = New-PowerBIWorkspace -Name $target_workspace_name -ErrorAction SilentlyContinue
    }

    if (!$target_workspace -or $target_workspace.isReadOnly -eq "True") {
        Write-Error "Invalid choice: you must have edit access to the workspace."
        break
    } else {
		$target_workspace_ID = $target_workspace.id
    }

    if(!$target_workspace_ID) {
        Write-Warning "Could not get a workspace with that name. Please try again with a different name."  
    } 
}

# ==================================================================
# PART 3: Copying reports and datasets via Export/Import of 
#         reports built on PBIXes (this step creates the datasets)
# ==================================================================
$report_ID_mapping = @{}      # mapping of old report ID to new report ID
$dataset_ID_mapping = @{}     # mapping of old model ID to new model ID

# STEP 3.1: Create a temporary folder to export the PBIX files.
$temp_path_root = "$PSScriptRoot\pbi-copy-workspace-temp-storage"
$temp_dir = New-Item -Path $temp_path_root -ItemType Directory -ErrorAction SilentlyContinue

# STEP 3.2: Get the reports from the source workspace
$reports = if($source_workspace_ID -eq "me") { Get-PowerBIReport } else { Get-PowerBIReport -WorkspaceId $source_workspace_ID }

# STEP 3.3: Export the PBIX files from the source and then import them into the target workspace
Foreach($report in $reports) {
   
    $report_id = [guid]$report.id
    $dataset_id = [guid]$report.datasetId
    $report_name = $report.name
    $temp_path = "$temp_path_root\$report_name.pbix"

    # Only export if this dataset if it hasn't already been exported already
    if ($dataset_ID_mapping -and $dataset_ID_mapping[$dataset_id]) {
        continue
    }

    Write-Host "== Exporting $report_name with id: $report_id to $temp_path"
    try {
        Export-PowerBIReport -Id $report_id -OutFile "$temp_path" -ErrorAction Stop
    } catch {
        Write-Warning "= This report and dataset cannot be copied, skipping. This is expected for most workspaces."
        continue
    }
     
     try {
        Write-Host "== Importing $report_name to target workspace"

        $new_report = New-PowerBIReport -WorkspaceId $target_workspace_ID -Path $temp_path -Name $report_name -ConflictAction Abort
                
        # Get the report again because the dataset id is not immediately available with New-PowerBIReport
        $new_report = Get-PowerBIReport -WorkspaceId $target_workspace_ID -Id $new_report.id
        if($new_report) {
            # keep track of the report and dataset IDs
            $report_id_mapping[$report_id] = $new_report.id
            $dataset_id_mapping[$dataset_id] = $new_report.datasetId
        }
    } catch [Exception] {
	    Write-Error "== Error: failed to import PBIX"

        $exception = Resolve-PowerBIError -Last
        Write-Error "Error Description:" $exception.Message
        continue
    }
}

# STEP 3.4: Copy any remaining reports that have not been copied yet. 
$failure_log = @()  

Foreach($report in $reports) {
    $report_name = $report.name
    $report_datasetId = [guid]$report.datasetId

    $target_dataset_Id = $dataset_id_mapping[$report_datasetId]
    if ($target_dataset_Id -and !$report_ID_mapping[$report.id]) {
         Write-Host "== Copying report $report_name"
        $report_copy = if ($source_workspace_ID -eq "me") { 
            Copy-PowerBIReport -Report $report -TargetWorkspaceId $target_workspace_ID -TargetDatasetId $target_dataset_Id } else {
            Copy-PowerBIReport -Report $report -WorkspaceId $source_workspace_ID -TargetWorkspaceId $target_workspace_ID -TargetDatasetId $target_dataset_Id }

        $report_ID_mapping[$report.id] = $report_copy.id
    } else {
         $failure_log += $report
    }
}

# ==================================================================
# PART 4: Copy dashboards and tiles
# ==================================================================

# STEP 4.1 Get all dashboards from the source workspace
# If source is My Workspace, filter out dashboards that I don't own - e.g. those shared with me
$dashboards = "" 
if ($source_workspace_ID -eq "me") {
    $dashboards = Get-PowerBIDashboard
    $dashboards_temp = @()
    Foreach($dashboard in $dashboards) {
        if ($dashboard.isReadOnly -ne "True") {
            $dashboards_temp += $dashboard
        }
    }
    $dashboards = $dashboards_temp
}
else {
    $dashboards = Get-PowerBIDashboard -WorkspaceId $source_workspace_ID
}

# STEP 4.2 Copy the dashboards and their tiles to the target workspace
Foreach ($dashboard in $dashboards) {
    $dashboard_id = $dashboard.id
    $dashboard_name = $dashboard.Name

    Write-Host "== Cloning dashboard: $dashboard_name"

    # create new dashboard in the target workspace
    $dashboard_copy = New-PowerBIDashboard -Name $dashboard_name -WorkspaceId $target_workspace_ID
    $target_dashboard_id = $dashboard_copy.id

    Write-Host " = Copying the tiles..." 
    $tiles = if ($source_workspace_ID -eq "me") { 
        Get-PowerBITile -DashboardId $dashboard_id } else {
        Get-PowerBITile -WorkspaceId $source_workspace_ID -DashboardId $dashboard_id }

    Foreach ($tile in $tiles) {
        try {
            $tile_id = $tile.id
            if($tile.reportId) {
                $tile_report_Id = [GUID]($tile.reportId)
            }
            else {
                $tile_report_Id = $null
            }

            if(!$tile.datasetId) {
                Write-Warning "= Skipping tile $tile_id, no dataset id..."
                continue
            }
            else {
                $tile_dataset_Id = [GUID]($tile.datasetId)
            }

            if ($tile_report_id) { $tile_target_report_id = $report_id_mapping[$tile_report_id] }
            if ($tile_dataset_id) { $tile_target_dataset_id = $dataset_id_mapping[$tile_dataset_id] }

            # clone the tile only if a) it is not built on a dataset or b) if it is built on a report and/or dataset that we've moved
            if (!$tile_report_id -Or $dataset_id_mapping[$tile_dataset_id]) {
                $tile_copy = if ($source_workspace_ID -eq "me") { 
                    Copy-PowerBITile -DashboardId $dashboard_id -TileId $tile_id -TargetDashboardId $target_dashboard_id -TargetWorkspaceId $target_workspace_ID -TargetReportId $tile_target_report_id -TargetDatasetId $tile_target_dataset_id } else {
                    Copy-PowerBITile -WorkspaceId $source_workspace_ID -DashboardId $dashboard_id -TileId $tile_id -TargetDashboardId $target_dashboard_id -TargetWorkspaceId $target_workspace_ID -TargetReportId $tile_target_report_id -TargetDatasetId $tile_target_dataset_id }
                
                Write-Host "." -NoNewLine
            } else {
                $failure_log += $tile
            } 
           
        } catch [Exception] {
            Write-Error "Error: skipping tile..."
            Write-Error $_.Exception
        }
    }
    Write-Host "Done!"
}

# ==================================================================
# PART 5: Cleanup
# ==================================================================
Write-Host "Cleaning up temporary files"
Remove-Item -path $temp_path_root -Recurse

