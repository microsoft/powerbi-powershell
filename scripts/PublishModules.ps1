##############################
#.SYNOPSIS
# Publishes PowerShell modules inside NuGet packages to the PowerShell Gallery. 
#
#.DESCRIPTION
# Takes an existing NuGet package containing PowerShell module and uploads to the PowerShell gallery through nuget.exe.
#
#.EXAMPLE
# PS:> .\PublishModules.ps1 -Path ..\PkgOut\Reset -ApiKey XXXXXXX
# Takes any *.nupkg files under PkgOut\Reset and publishes them to PowerShell Gallery.
#
#.NOTES
# Optionally, you could use Publish-Module to do this task but that cmdlet doesn't support already packaged modules (as it reflects against a *.psd1 file to package).
# This script requires nuget.exe to be available in your $env:Path.
##############################
[CmdletBinding(SupportsShouldProcess=$true)]
param
(
    # Parent directory containing *.nupkg files to publish
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $Path,

    # API Key to publish to NuGet feed (PowerShell Gallery).
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $ApiKey,

    # NuGet feed to publish to. Defaults to https://www.powershellgallery.com/api/v2/package/.
    [ValidateNotNullOrEmpty()]
    [string] $NuGetSource = 'https://www.powershellgallery.com/api/v2/package/'
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"

# This file mimics behavior from C:\Program Files\WindowsPowerShell\Modules\PowerShellGet\<version>\PSModule.psm1

if(!(Test-Path -Path $Path)) {
    throw "-Path not found: $Path"
}

$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop

# Sorting by CreationTime as this should represent the order of construction so dependency order is followed for publishing
$nupkgFiles = Get-ChildItem -Path $Path -Filter *.nupkg -Recurse | Sort-Object CreationTime
if(!$nupkgFiles) {
    throw "No *.nupkg files found under: $Path"
}

Write-Output "Nuget packages to Publish:`n$($nupkgFiles | ForEach-Object { $_.FullName } | Out-String)"
foreach($nupkgFile in $nupkgFiles) {
    $nugetFilePath = $nupkgFile.FullName
    $nugetPackageName = $nupkgFile.Name

    $nugetArgs = @('push', "$nugetFilePath", '-Source', $NuGetSource, '-ApiKey', $ApiKey, '-NonInteractive')
    
    if($PSCmdlet.ShouldProcess($nupkgFile, "Publish NuGet Package")) {
        Write-Output "Publishing package: $nugetFilePath"
        & $nugetExe $nugetArgs
        if($LASTEXITCODE -ne 0) {
            throw "Failed to publish package $nugetPackageName"
        }

        Write-Output "Pushed package $nugetPackageName to $NuGetSource"
    }
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"