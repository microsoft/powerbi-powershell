##############################
#.SYNOPSIS
# Downloads *.nupkg files from AppVeyor.
#
#.DESCRIPTION
# Downloads and unpacks *.nupkg files on AppVeyor from powerbi-powershell builds.
#
#.EXAMPLE
# PS:> .\DownloadNupkg.ps1 -OutputDirectory ..\Out
# Downloads the latest package on AppVeyor to ..\Out directory.
#
#.NOTES
# Uses the nuget.exe, must be discoverable in $env:Path.
##############################
[CmdletBinding()]
param
(
    # Output directory of modules.
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectory,

    # NuGet feed source. Default is 'https://ci.appveyor.com/nuget/powerbi-powershell-j0f50attoqd6'.
    [ValidateNotNullOrEmpty()]
    [string] $NugetSource = "https://ci.appveyor.com/nuget/powerbi-powershell-j0f50attoqd6",

    # Nuget packages to pull. Package dependencies will also get pulled if using a newer version of NuGet.exe. If null, it will be discovered from NugetSource. Default is 'MicrosoftPowerBIMgmt'.
    [string[]] $PackageNames = @('MicrosoftPowerBIMgmt'),

    # Package version to use. If null, latest version is used.
    [string] $PackageVersion,

    # Indicates to use Prerelease package version.
    [switch] $Prerelease
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"
$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop

if(!(Test-Path -Path  $OutputDirectory)) {
    [void](New-Item -Path $OutputDirectory -ItemType Directory -Force)
}

if(!$PackageNames -or $PackageNames.Count -lt 1) {
    Write-Output "Determining package names..."
    $nugetArgs = @('list', '-Source', $NugetSource, '-ForceEnglishOutput', '-NonInteractive')
    if($PackageVersion) {
        $nugetArgs += '-AllVersions'
    }

    if($Prerelease) {
        $nugetArgs += '-Prerelease'
    }

    Write-Verbose "Running: & $nugetExe $($nugetArgs -join ' ')"
    $nugetPackages = & $nugetExe $nugetArgs
    if($LASTEXITCODE -ne 0) {
        throw "Nuget.exe failed with exit code: $LASTEXITCODE"
    }

    $groupedPackgesByVersion = $nugetPackages | ForEach-Object { $t = $_ -split ' '; [pscustomobject]@{Name=$t[0]; Version=$t[1] }} | Group-Object -Property Version -AsHashTable
    if($PackageVersion) {
        $PackageNames = ($groupedPackgesByVersion[$PackageVersion]).Name
    }
    else {
        $PackageNames = ($groupedPackgesByVersion.Values).Name
    }
}

Write-Output "Installing the nuget package(s) '$($PackageNames -join ', ')' from source '$NugetSource' to output directory '$OutputDirectory'"

foreach($packageName in $PackageNames) {
    Write-Output "Getting package: $packageName"
    $nugetArgs = @('install', $packageName, '-OutputDirectory', $OutputDirectory, '-Source', $NugetSource, '-PackageSaveMode', 'nuspec', '-NonInteractive')
    
    if($PackageVersion) {
        $nugetArgs += @('-Version', $PackageVersion)
    }
    
    if($Prerelease) {
        $nugetArgs += '-Prerelease'
    }

    Write-Verbose "Running: & $nugetExe $($nugetArgs -join ' ')"
    & $nugetExe $nugetArgs
    if($LASTEXITCODE -ne 0) {
        throw "Nuget.exe failed with exit code: $LASTEXITCODE"
    }

    # $nugetPackageDirectory = Get-ChildItem -Path $OutputDirectory | Where-Object { $_.Name -match "$([regex]::Escape($packageName)).\d+.*" }
    # if(!$nugetPackageDirectory) {
    #     throw "Failed to locate package '$packageName' in directory: $OutputDirectory"
    # }

    # Get-ChildItem -Path $nugetPackageDirectory.FullName -Exclude *.nupkg | Remove-Item -Recurse -Force
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"