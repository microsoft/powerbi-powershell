<#
.Synopsis
    Migrates users to Pro Power BI licenses.
.Description
    Migrates standard\free Power BI licensed users to Power BI Pro licenses.
    This script uses the MSOnline PowerShell module. if this module isn't installed it will be installed into your the current user profile.
.Parameter Path
    Path to CSV containing users (PUIDs) to migrate to Power BI Pro licenes.
.Parameter PUIDColumnName
    Name of column in CSV that contains the PUID for the user to migrate. Default is 'Puid'.
.Parameter ProLicenseName
    Name of Pro Power BI license. Default is 'POWER_BI_PRO'.
.Parameter NoResultFile
    Indicates to not write result files for the execution of the script. Result still shows in console output.
.Example
    PS C:\> .\MigrateUsersToPro.ps1 -Path .\usersToMigrate.csv

    CSV has a column named 'Puid', IDs in column will be migrated to Pro Power BI licesnes (if possible).
#>
[CmdletBinding()]
param
(
    [Parameter(Mandatory)]
    $Path,

    [ValidateNotNullOrEmpty()]
    [string] $PUIDColumnName = 'Puid',

    [ValidateNotNullOrEmpty()]
    [string] $ProLicenseName = 'POWER_BI_PRO',

    [switch] $NoResultFile
)

#region Helper Functions

function Assert-ModuleExists([string]$ModuleName) {
    $msonlineModule = Get-Module $ModuleName -ListAvailable -ErrorAction SilentlyContinue
    if(!$msonlineModule) {
        Write-Host "Installing module $ModuleName ..."
        Install-Module -Name $ModuleName -Force -Scope CurrentUser
        Write-Host "Module installed"
    }
}

#endregion

# Load CSV
$migrateCsv = Import-Csv -Path $Path -ErrorAction Stop
$userPuids = [string[]]($migrateCsv."$PUIDColumnName")
if($userPuids.Count -eq 0) {
    throw "Unable to pull PUIDs from CSV file '$Path' with column name '$PUIDColumnName' (-PUIDColumnName)"
}

$csvDirectory = (Get-Item -Path $Path).Directory.FullName

# Install\assert external module exists
Assert-ModuleExists -ModuleName MSOnline

# Login to O365
Write-Host "Login with Office 365 administrator"
Connect-MsolService

# Load SKUs
$skus = Get-MsolAccountSku
$skus | Out-Host

# Locate Pro Sku
$proSku = $skus | Where-Object { $_.AccountSkuId.endswith($ProLicenseName) } | Select-Object -first 1
if(!$proSku) {
    throw "Failed to find Power BI Pro SKU"
}

# Detect if all Pro licenses are consumed
if($proSku.ActiveUnits -le $proSku.ConsumedUnits) {
    Write-Warning "You are currently consuming all active units of your Power BI Pro license, please remove users from licenses and try again"
    Write-Warning $proSku
    return
}

# Indicate some users can't be migrated due to lack of Pro licenses
$availableProLicenses = $proSku.ActiveUnits - $proSku.ConsumedUnits
Write-Host "Number of available pro licenses: $availableProLicenses"
if($availableProLicenses -lt $userPuids.Count) {
    Write-Warning "Not enough licenses to migrate all users"
}

# Show uses that will be migrated
Write-Host "Migrating users with PUIDs:"
$userPuids | Out-Host

# Filter out any non-active (enabled) users
$enabledUsers = Get-MsolUser -EnabledFilter EnabledOnly

# Look up standard Users to migrate and pro users (already migrated)
$standardUsers = $enabledUsers | Where-Object { $assignedSkus = (($_.LicenseAssignmentDetails).AccountSku).SkuPartNumber; $ProLicenseName -notin $assignedSkus }
$proUsers = $enabledUsers | Where-Object { $assignedSkus = (($_.LicenseAssignmentDetails).AccountSku).SkuPartNumber; $ProLicenseName -in $assignedSkus }

$usersMigrated = @()
$alreadyPro = @()
$failedToMigrate = @()
foreach($userPuid in $userPuids) {
    $isPro = $proUsers | Where-Object LiveID -eq $userPuid | Select-Object -First 1
    if($isPro) {
        Write-Warning "User with PUID '$userPuid' is already a PRO user"
        $alreadyPro += $isPro
        continue
    }

    $isStandard = $standardUsers | Where-Object LiveID -eq $userPuid | Select-Object -First 1
    if(!$isStandard) {
        Write-Error "User with PUID '$userPuid' is not a standard user which is needed to migrate to pro"
        $failedToMigrate += ($enabledUsers | Where-Object LiveId -eq $userPuid | Select-Object -First 1)
        continue
    }

    if($availableProLicenses -gt 0) {
        Set-MsolUserLicense -ObjectId $isStandard.ObjectId -AddLicenses @($proSku.AccountSkuId)
        $usersMigrated += $isStandard
        $availableProLicenses--
    }
    else {
        Write-Error "User with PUID '' was not migrated to pro because available licenses was zero"
        $failedToMigrate += $isStandard
    }
}

Write-Host "Finished migrating users to pro licenses"

if($usersMigrated.Count -gt 0) {
    Write-Host "$($usersMigrated.Count) users were migrated to pro licenses" -ForegroundColor Green
    $usersMigrated | Out-Host

    if(!$NoResultFile) {
        # Append to handle multiple runs
        $migratedUserFilePath = Join-Path -Path $csvDirectory -ChildPath 'migratedUsersToPro.csv'
        Write-Host "`nUsers migrated to pro written to: $migratedUserFilePath"
        $usersMigrated | Export-Csv -Path $migratedUserFilePath -Force -Append -NoTypeInformation -ErrorAction Continue
    }
}
else {
    Write-Warning "Summary: No users were migrated to pro licenses"
}

if($alreadyPro.Count -gt 0) {
    Write-Warning "$($alreadyPro.Count) users already had pro licenses"
    $alreadyPro | Out-Host
    
    if(!$NoResultFile) {
        # Append for multiple runs
        $alreadyProFilePath = Join-Path -Path $csvDirectory -ChildPath 'alreadyProUsers.csv'
        Write-Host "`nAlready pro users written to: $alreadyProFilePath"
        $alreadyPro | Export-Csv -Path $alreadyProFilePath -Force -Append -NoTypeInformation -ErrorAction Continue
    }
}
else {
    Write-Host "Summary: No users found that already had pro licenses" -ForegroundColor Green
}

if($failedToMigrate.Count -gt 0) {
    Write-Error "Failed to migrate $($failedToMigrate.Count) users to pro licenses"
    $failedToMigrate | Out-Host

    if(!$NoResultFile) {
        # No appending to keep file fresh from past run
        $failedToMigrateFilePath = Join-Path -Path $csvDirectory -ChildPath 'filedToMigrateUsers.csv'
        Write-Host "`nFailed to migrate users written to: $failedToMigrateFilePath"
        $failedToMigrate | Export-Csv -Path $failedToMigrateFilePath -Force -NoTypeInformation -ErrorAction Continue
    }
}
else {
    Write-Host "Summary: No users failed to migrate to pro" -ForegroundColor Green
}