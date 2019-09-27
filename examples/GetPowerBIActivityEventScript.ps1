<#

.Synopsis
    Retrieves the audit activity events for a Power BI tenant. 
.Description
    Retrieves the audit activity events for a Power BI tenant with in a time window.
    This script uses the MicrosoftPowerBIMgmt.Profile PowerShell module. if this module isn't installed it will be installed into your current user profile.
.Parameter StartDateTime
    Specifies the start of a timespan to retrieve audit activity events. It should be in UTC format and ISO 8601 compliant.
    Both StartDateTime and EndDateTime should be within the same UTC day.
.Parameter EndDateTime
    Specifies the end of a timespan to retrieve audit activity events. It should be in UTC format and ISO 8601 compliant.
    Both StartDateTime and EndDateTime should be within the same UTC day.
.Parameter ActivityType
    Filters the activity records based on this activity type.
.Parameter User
    Filters the activity records based on this user email.
.Example
    PS C:\> .\GetPowerBIActivityEventScript.ps1 -StartDateTime 2019-09-12T00:00:00 -EndDateTime 2019-09-12T10:00:00 -ActivityType viewreport -User admin@contoso.com

#>

[CmdletBinding()]
param
(
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $StartDateTime,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $EndDateTime,

    [string] $ActivityType,

    [string] $User
)

#region Helper Functions 

function Assert-ModuleExists([string]$ModuleName) {
    $module = Get-Module $ModuleName -ListAvailable -ErrorAction SilentlyContinue
    if (!$module) {
        Write-Host "Installing module $ModuleName ..."
        Install-Module -Name $ModuleName -Force -Scope CurrentUser
        Write-Host "Module installed"
    }
    elseif ($module.Version -ne '1.0.0' -and $module.Version -le '1.0.410') {
        Write-Host "Updating module $ModuleName ..."
        Update-Module -Name $ModuleName -Force -ErrorAction Stop
        Write-Host "Module updated"
    }
}

Assert-ModuleExists -ModuleName "MicrosoftPowerBIMgmt.Profile"
Login-PowerBI

$result = @() # empty array to dump activity events into.

$UriPrefix = "admin/activityevents?"
$OriginalUrl = [string]::Format("{0}startDateTime='{1}'&endDateTime='{2}'",$UriPrefix,$StartDateTime,$EndDateTime)

if (!([string]::IsNullOrEmpty($ActivityType)) -and ([string]::IsNullOrEmpty($User))) {
    $OriginalUrl = [string]::Format("{0}&`$filter=Activity eq '{1}'",$OriginalUrl,$ActivityType)
}
elseif (([string]::IsNullOrEmpty($ActivityType)) -and !([string]::IsNullOrEmpty($User))) {
    $OriginalUrl = [string]::Format("{0}&`$filter=UserId eq '{1}'",$OriginalUrl,$User)
}
elseif (!([string]::IsNullOrEmpty($ActivityType)) -and !([string]::IsNullOrEmpty($User))) {
    $OriginalUrl = [string]::Format("{0}&`$filter=Activity eq '{1}' and UserId eq '{2}'",$OriginalUrl,$ActivityType,$User)
}

$activityEvents = (Invoke-PowerBIRestMethod -Url $OriginalUrl -Method Get) | ConvertFrom-Json
$result = $result + $activityEvents.activityEventEntities
$continuationUri = $activityEvents.continuationUri
$continuationToken = $activityEvents.continuationToken

# Checking on both continuationUri and continuationToken since the continuationUri changes are still flighting across rings at the time of this script's check in.
while ($continuationUri -or $continuationToken) {
    $FullUri = [string]::Empty
    if (![string]::IsNullOrEmpty($continuationUri)) {
        $UriAndQuerySplit = $continuationUri -split "\?"
        $FullUri = [string]::Format("{0}{1}",$UriPrefix,$UriAndQuerySplit[1])    
    }
    elseif (![string]::IsNullOrEmpty($continuationToken)) {
        $FullUri = [string]::Format("{0}&continuationToken='{1}'",$OriginalUrl,$continuationToken)
    }
    
    $activityEvents = (Invoke-PowerBIRestMethod -Url $FullUri -Method Get) | ConvertFrom-Json
    $result = $result + $activityEvents.activityEventEntities
    $continuationUri = $activityEvents.continuationUri
    $continuationToken = $activityEvents.continuationToken
}

$result