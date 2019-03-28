# PART 1: Authentication
# ==================================================================
 Login-PowerBI

# PART 2: Getting source and target workspace
# ==================================================================
# STEP 2.1: Get the source workspace
$source_group_ID = ""
while (!$source_group_ID) {
    $source_group_name = Read-Host -Prompt "Enter the name of the workspace you'd like to copy from"

    if($source_group_name -eq "My Workspace") {
        $source_group_ID = "me"
        break
    }

    $group = Get-PowerBIWorkspace -Name $source_group_name -ErrorAction SilentlyContinue
    if (!$group -or $group.isReadOnly -eq "True") {
        "Invalid choice: you must have edit access to the workspace"
        break
    } else {
        $source_group_ID = $group.id
        break
    }

    if(!$source_group_ID) {
        "Please try again, making sure to type the exact name of the workspace"  
    } 
}

# STEP 2.2: Get the target workspace
$target_group_ID = "" 
while (!$target_group_ID) {
    $target_group_name = Read-Host -Prompt "Enter the name of the workspace you'd like to copy to"

    $target_group = Get-PowerBIWorkspace -Name $target_group_name -ErrorAction SilentlyContinue
    if(!$target_group) {
        $target_group = New-PowerBIWorkspace -Name $target_group_name -ErrorAction SilentlyContinue
    }

    $target_group_ID = $target_group.id

    if(!$target_group_ID) {
        "Could not get a workspace with that name. Please try again with a different name."  
    } 
}

# PART 3: Copying reports and datasets via Export/Import of 
#         reports built on PBIXes (this step creates the datasets)
# ==================================================================
$report_ID_mapping = @{}      # mapping of old report ID to new report ID
$dataset_ID_mapping = @{}     # mapping of old model ID to new model ID

# STEP 3.1: Create a temporary folder to export the PBIX files.
$temp_path_root = "$PSScriptRoot\pbi-copy-workspace-temp-storage"
$temp_dir = New-Item -Path $temp_path_root -ItemType Directory -ErrorAction SilentlyContinue

# STEP 3.2: Get the reports from the source workspace
$reports = if($source_group_ID -eq "me") { Get-PowerBIReport } else { Get-PowerBIReport -WorkspaceId $source_group_ID }

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

    "== Exporting $report_name with id: $report_id to $temp_path"
    try {
        Export-PowerBIReport -Id $report_id -OutFile "$temp_path" -ErrorAction Stop
    } catch {
        Write-Host "= This report and dataset cannot be copied, skipping. This is expected for most workspaces."
        continue
    }
     
     try {
        "== Importing $report_name to target workspace"

        $new_report = New-PowerBIReport -WorkspaceId $target_group_id -Path $temp_path -Name $report_name -ConflictAction Abort
                
        # Get the report again because the dataset id is not immediately available with New-PowerBIReport
        $new_report = Get-PowerBIReport -WorkspaceId $target_group_id -Id $new_report.id
        if($new_report) {
            # keep track of the report and dataset IDs
            $report_id_mapping[$report_id] = $new_report.id
            $dataset_id_mapping[$dataset_id] = $new_report.datasetId
        }
    } catch [Exception] {
	    Write-Host "== Error: failed to import PBIX"

        $exception = Resolve-PowerBIError -Last
        Write-Host "Error Description:" $exception.Message
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
        "== Copying report $report_name"
        $report_copy = if ($source_group_ID -eq "me") { 
            Copy-PowerBIReport -Report $report -TargetWorkspaceId $target_group_id -TargetDatasetId $target_dataset_Id } else {
            Copy-PowerBIReport -Report $report -WorkspaceId $source_group_ID -TargetWorkspaceId $target_group_id -TargetDatasetId $target_dataset_Id }

        $report_ID_mapping[$report.id] = $report_copy.id
    } else {
         $failure_log += $report
    }
}


# PART 4: Copy dashboards and tiles
# ==================================================================

# STEP 4.1 Get all dashboards from the source workspace
# If source is My Workspace, filter out dashboards that I don't own - e.g. those shared with me
$dashboards = "" 
if ($source_group_ID -eq "me") {
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
    $dashboards = Get-PowerBIDashboard -WorkspaceId $source_group_ID
}

# STEP 4.2 Copy the dashboards and their tiles to the target workspace
Foreach ($dashboard in $dashboards) {
    $dashboard_id = $dashboard.id
    $dashboard_name = $dashboard.Name

    "== Cloning dashboard: $dashboard_name"

    # create new dashboard in the target workspace
    $dashboard_copy = New-PowerBIDashboard -Name $dashboard_name -WorkspaceId $target_group_id
    $target_dashboard_id = $dashboard_copy.id

    " = Copying the tiles..." 
    $tiles = if ($source_group_ID -eq "me") { 
        Get-PowerBITile -DashboardId $dashboard_id } else {
        Get-PowerBITile -WorkspaceId $source_group_ID -DashboardId $dashboard_id }

    Foreach ($tile in $tiles) {
        try {
            $tile_id = $tile.id
            $tile_report_Id = [GUID]($tile.reportId)
            $tile_dataset_Id = [GUID]($tile.datasetId)
            if ($tile_report_id) { $tile_target_report_id = $report_id_mapping[$tile_report_id] }
            if ($tile_dataset_id) { $tile_target_dataset_id = $dataset_id_mapping[$tile_dataset_id] }

            # clone the tile only if a) it is not built on a dataset or b) if it is built on a report and/or dataset that we've moved
            if (!$tile_report_id -Or $dataset_id_mapping[$tile_dataset_id]) {
                $tile_copy = if ($source_group_ID -eq "me") { 
                    Copy-PowerBITile -DashboardId $dashboard_id -TileId $tile_id -TargetDashboardId $target_dashboard_id -TargetWorkspaceId $target_group_id -TargetReportId $tile_target_report_id -TargetDatasetId $tile_target_dataset_id } else {
                    Copy-PowerBITile -WorkspaceId $source_group_ID -DashboardId $dashboard_id -TileId $tile_id -TargetDashboardId $target_dashboard_id -TargetWorkspaceId $target_group_id -TargetReportId $tile_target_report_id -TargetDatasetId $tile_target_dataset_id }
                
                Write-Host "." -NoNewLine
            } else {
                $failure_log += $tile
            } 
           
        } catch [Exception] {
            "Error: skipping tile..."
            Write-Host $_.Exception
        }
    }
    "Done!"
}

"Cleaning up temporary files"
Remove-Item -path $temp_path_root -Recurse

