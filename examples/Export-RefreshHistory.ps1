<#
.Synopsis
	Export Refresh History.
.Description
	This script exports the refresh history.
    You can select a special workspace or all workspaces you are member.
.Parameter Path
    Folder in which the export files will be stored.
    Default: Path to $env:temp - example: c:\users\johndoe\appdata\local\temp
.Parameter Json
    Defines if a JSON file with full output will be created.
    By Default JSON file will be created.
.Parameter JsonFile
    Name of JSON file.
    By Default JSON file will be names like 'log_yyyy_MM_dd_hh_mm_ss.json' f.e 'log_2018_09_10_12_12_00.json'.
.Parameter Csv
    Defines if a CSV file with limited output will be created.
    By Default CSV file will be created.
.Parameter CsvFile
    Name of CSV file.
    By Default CSV file will be names like 'log_yyyy_MM_dd_hh_mm_ss.csv' f.e 'log_2018_09_10_12_12_00.csv'.
.Parameter Dedicated
    Defines if only dedicated workspaces will be looked at.
    By default all workspaces will be logged.
.Parameter AdminMode
    Gets all workspaces if no workspace name is selected.
    By default off.
.Parameter WorkspaceName
    Name of Workspace - if no workspace is set, all workspaces will be logged.
.Parameter DatasetName
    Name of dataset - if no dataset name is set, the refresh history of all datasets in a workspace will be logged.
.Parameter Scope
    How many entries per dataset (All | Today). Default is 'All'.
.Parameter Top
    How many entries per dataset.
.Example
	PS C:\> Export-RefreshHistory 

    Exports the refresh history of all datasets of all workspace into $env:TEMP folder
.Example
	PS C:\> Export-RefreshHistory -Path "c:\refresh\Sales" -WorkspaceName "Sales" -Json $false

    Exports the refresh history of all datasets of workspace Sales into c:\refresh\Sales
    Creates only a CSV file
.Example
	PS C:\> Export-RefreshHistory -Path "c:\refresh\Sales" -WorkspaceName "Sales" -DatasetName "Sales Bikes" -Scope Today

    Exports the refresh history of dataset "Sales Bikes" of workspace "Sales" into folder "c:\refresh\Sales" only for the current day
#>
param(
    [Alias('TargetFolder')]
    [string] $Path = $env:TEMP,

    [bool] $Json = $true,

    [string] $JsonFile = 'Log_' + (Get-Date).ToString("yyy_MM_dd_hh_mm_ss") + '.json',

    [bool] $Csv = $true,

    [string] $CsvFile = 'Log_' + (Get-Date).ToString("yyy_MM_dd_hh_mm_ss") + '.csv',

    [bool] $Dedicated = $false,

    [bool] $AdminMode = $false,

    [string] $WorkspaceName,

    [string] $DatasetName,

    [ValidateSet('All', 'Today')]
    [string] $Scope = 'All',

    [int] $Top
)

function Get-DetailValue($details, $key)
{
    foreach ($detail in $details)
    {
        if ($detail.code -eq $key)
        {
            return $detail.detail.value
        }
    }
}

function Get-RefreshHistory
{
    param
    (
        [Microsoft.PowerBI.Common.Api.Workspaces.Workspace] $Workspace,

        [Microsoft.PowerBI.Common.Api.Datasets.Dataset] $Dataset,

        [int] $Top,

        [ValidateSet('All','Today')]
        [string] $Scope
    )

    # get url
    $url = "groups/$($Workspace.id)/datasets/$($Dataset.Id)/refreshes"
    Write-Verbose "Get-RefreshHistory:Workspace:$($Workspace.Name);Dataset:$($Dataset.Name)"
    Write-Verbose "Get-RefreshHistory:Url:$url"
    if ($scope)
    {
        # get history objects collection based on scope 'All' - all records, 'Today' - todays redreshes
        $history = ((Invoke-PowerBIRestMethod -Url $url -Method Get) |
            ConvertFrom-Json).value |
            Where-Object { ($scope -eq "all") -or (($scope -eq "today") -and ($_.startTime.SubString(0, 10) -eq ((Get-Date).ToString("yyyy-MM-dd"))))}
            Sort-Object -Descending -Property timeStart
    }
    elseif ($Top)
    {
        # get history objects collection based on Top claus like ODATA/SQL
        $history = ((Invoke-PowerBIRestMethod -Url $url -Method Get) |
            ConvertFrom-Json).value |
            Sort-Object -Descending -Property timeStart |
            Select-Object -First $Top
    }
    return $history
}

# check if Path does exist
if (!(Test-Path -Path $Path))
{
    throw "Path '$Path' not found"
}
else
{
    Write-Host "Log files will be stored in folder '$Path' ..."
}

Login-PowerBI | Out-Null

if ($WorkspaceName) # check if WorkspaceName set
{
    # get special workspace
    $workspaces = @(Get-PowerBIWorkspace -Name $WorkspaceName)
}
elseif ($Dedicated) # only workspaces in capacity
{
    $workspaces = Get-PowerBIWorkspace |
        Where-Object { $_.IsOnDedicatedCapacity -and (!$_.IsReadOnly)}
}
else # get all workspaces
{
    if ($AdminMode) # if admin Mode all workspaces will be retrieved
    {
        $workspaces = Get-PowerBIWorkspace -Scope Organization |
            Where-Object { (!$_.IsReadOnly) } 
    } 
    else # only workspaces I am admin
    {   
        $workspaces = Get-PowerBIWorkspace |
            Where-Object { (!$_.IsReadOnly) }
    }
}

# create empty history array
$historyItems = @()

# go thru workspaces
foreach ($ws in $workspaces)
{
    Write-Verbose "Workspace: $($ws.name)"
    if ($DatasetName) # special dataset
    {
        # get special dataset
        $datasets = @(Get-PowerBIDataset -Workspace $ws |
            Where-Object { $_.Name -eq $DatasetName })
    }
    else
    {
        # get all datasets in workspace
        $datasets = Get-PowerBIDataset -Workspace $ws
    }

    # check for a valid dataset otherwise use next workspace
    if (!$datasets)
    { 
        Write-Host "No dataset(s) found in Workpace '$($ws.Name)' for '$DatasetName'" -ForegroundColor Red
        continue
    }

    # filter out 'report usage metrics model' dataset - always there
    foreach ($ds in ($datasets | Where-Object { $_.Name -ne "Report Usage Metrics Model"}) )
    {
        Write-Verbose "Dataset: $($ds.Name)"
        # successful refreshes
        $historyItems += Get-RefreshHistory -Workspace $ws -Dataset $ds -Scope $Scope -Top $Top | 
            Where-Object {$_.serviceExceptionJSON} |
            Select-Object -Property id, refreshType, startTime, endTime, status, serviceExceptionJson, `
                @{n='WorkspaceName';e={$ws.Name}}, @{n='WorkspaceId';e={$ws.Id}}, @{n='DatasetName';e={$ds.Name}}, @{n='DatasetId';e={$ds.Id}},`
                @{n='ErrorCode';e={""}}, @{n='ClusterUri';e={""}}, @{n='ActivityId';e={""}}, @{n='RequestId';e={""}}, @{n='Timestamp';e={""}}
        
        # failing refreshes
        $historyItems += Get-RefreshHistory -Workspace $ws -Dataset $ds -Scope $Scope -Top $Top | 
            Where-Object {-not ($_.serviceExceptionJSON)} |
            Select-Object -Property id, refreshType, startTime, endTime, status, @{n='serviceExceptionJson';e={$null}},`
                @{n='WorkspaceName';e={$ws.Name}}, @{n='WorkspaceId';e={$ws.Id}}, @{n='DatasetName';e={$ds.Name}}, @{n='DatasetId';e={$ds.Id}},`
                @{n='ErrorCode';e={""}}, @{n='ClusterUri';e={""}}, @{n='ActivityId';e={""}}, @{n='RequestId';e={""}}, @{n='Timestamp';e={""}}
    }

}

# add additional infos for failing refreshes
for ($i=0; $i -lt $historyItems.Length; $i++)
{
    if ($historyItems[$i].serviceExceptionJson)
    {
        # convert serviceException JSON into object
        $historyObject = ($historyItems[$i].serviceExceptionJson | ConvertFrom-Json)

        # fill the predefined error entries
        $historyItems[$i].ErrorCode = $historyObject.error.code
        $historyItems[$i].ClusterUri = Get-DetailValue -details $historyObject.error.'pbi.error'.details  -key "ClusterUriText"
        $historyItems[$i].ActivityId = Get-DetailValue -details $historyObject.error.'pbi.error'.details  -key "ActivityIdText"
        $historyItems[$i].RequestId = Get-DetailValue -details $historyObject.error.'pbi.error'.details  -key "RequestIdText"
        $historyItems[$i].Timestamp = Get-DetailValue -details $historyObject.error.'pbi.error'.details  -key "TimestampText"
    }
}

# create CSV file - remove serviceException column
if ($Csv) 
{
    $csvFilePath = "$Path\$CsvFile"
    $historyItems |
        Select-Object -Property id, refreshType, startTime, endTime, status, WorkspaceName,  WorkspaceId, DatasetName, DatasetId,`ErrorCode, ClusterUri,ActivityId,RequestId, Timestamp |
        Export-Csv -Path $csvFilePath -Delimiter ";" -Force -NoTypeInformation
    Write-Host "CSV log file '$csvFilePath' createds"
}

# create JSON file
if ($Json)
{
    $jsonFilePath = "$Path\$JsonFile"
    $historyItems |
        ConvertTo-Json -Depth 4 |
        Out-File -FilePath $jsonFilePath -Force
    Write-Host "JSON log file '$jsonFilePath' created"
}