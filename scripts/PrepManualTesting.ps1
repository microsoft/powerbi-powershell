##############################
#.SYNOPSIS
# Prepares PowerShell modules to be loaded in PowerShell prompt\console for end-to-end manual testing.
#
#.DESCRIPTION
# Executes a set of steps to mimic what the product looks like being delivered to customers through the PowerShell Gallery. 
# After script completes, you can call the cmdlets as they should be delay-loaded\imported when invoked.
#
#.EXAMPLE
# PS:> .\PrepManualTesting.ps1
# Takes an existing build output in -PackageDir path and loads it so it can be tested manually.
#
#.EXAMPLE
# PS:> .\PrepManualTesting.ps1 -Build
# Executes a build of the codebase, and uses that output to load into your executing command prompt so it can be tested manually. 
#
##############################
[CmdletBinding(SupportsShouldProcess)]
param
(
    # Indicates to build packages before prepping packages to load. Required if you haven't previously built packages by calling Pack target.
    [switch] $Build,

    # Default arguments to call MSBuild if -Build is specified.
    [ValidateNotNull()]
    [hashtable] $BuildScriptArgs = @{'Pack'=$true; 'Clean'=$true; 'Verbose'=$true},

    # Package output directory to prep for loading.
    [string] $PackageDir = "$PSScriptRoot\..\PkgOut",

    # Indicates to force cleaning PackageDir of any files not related to the modules (prevents being prompted to clean).
    [switch] $Force,

    # Indicates to only load cmdlets out of PackageDir.
    [switch] $LoadOnly
)

if($Build) {
    & "$PSScriptRoot\Build.ps1" @BuildScriptArgs
    if($LASTEXITCODE -ne 0) {
        throw "MSBuild failed with exit code $LASTEXITCODE"
    }
}

if(!(Test-Path -Path $PackageDir)) {
    throw "Directory '$PackageDir' does not exist, specify -Build to generate"
}

$PackageDir = (Resolve-Path -Path $PackageDir).ProviderPath
if(!$LoadOnly) {
    $nupkgFiles = Get-ChildItem -Path $PackageDir -Filter *.nupkg
    if(!$nupkgFiles) {
        throw "No NUPKG files found, specify -Build to generate"
    }

    $nonNuPkgFiles = Get-ChildItem -Path $PackageDir -Exclude *.nupkg
    if($nonNuPkgFiles) {
        if(!$Force -and !$PSCmdlet.ShouldContinue("$($nonNuPkgFiles | ForEach-Object { $_.FullName } | Out-String)", "Delete files")) {
            exit 0
        }

        Write-Verbose "Removing non-NUPKG files..."
        $nonNuPkgFiles | Remove-Item -Force -ErrorAction Stop -Recurse
    }

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $nupkgFiles | ForEach-Object {
        $moduleName = $_.BaseName -split '\.\d' | Select-Object -First 1
        Write-Verbose "Unpacking zip '$($_.Name)' to directory '$moduleName'..."
        $newName = Rename-Item -Path $_.FullName -NewName "$moduleName.zip" -Force -PassThru
        $outDir = Join-Path -Path $PackageDir -ChildPath $moduleName
        [System.IO.Compression.ZipFile]::ExtractToDirectory($newName.FullName, $outDir)
    }
}

if($env:PSModulePath -split ';' -inotcontains $PackageDir) {
    Write-Verbose "Adding '$PackageDir' to PSModulePath environment variable..."
    $speraterChar = ';'
    if ($IsLinux -or $IsMacOS) {
        $speraterChar = ':'
    }

    $env:PSModulePath = $env:PSModulePath.TrimEnd($speraterChar)
    $env:PSModulePath += "$($speraterChar)$PackageDir"
}

Write-Output "[Done] Updated PSModulePath with '$PackageDir', you can now invoke cmdlets (causes module auto-loading)"