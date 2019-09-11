##############################
#.SYNOPSIS
# Executes a command-line (CLI) build of the PowerBIPowerShell solution.
#
#.DESCRIPTION
# Invokes MSBuild by locating it and passing common properties and targets for the PowerBIPowerShell solution
#
#.EXAMPLE
# PS:> .\Build.ps1
# Executes just build for the solution.
#
#.EXAMPLE
# PS:> .\Build.ps1 -Pack -Clean
# Executes build, pack and clean for the solution.
#
#.NOTES
# Requires Visual Studio 2017 to be installed (at least 15.2 where vswhere.exe is available).
##############################
[CmdletBinding()]
param
(
    # Path to solution file. Defaults to <script dir>\src\PowerBIPowerShell.sln.
    [ValidateNotNullOrEmpty()]
    [string] $Solution = "$PSScriptRoot\..\src\PowerBIPowerShell.sln",

    # MSBuild targets to execute. Default is the Build target unless -NoBuild is specified.
    [ValidateNotNull()]
    [string[]] $MSBuildTargets = @(),

    # MSBuild properties to execute build with. Default is @{'GenerateFullPaths'='true'}.
    [ValidateNotNull()]
    [Hashtable] $MSBuildProperties = @{'GenerateFullPaths'='true'},

    # Build Configuration. Default is to use the MSBuild project defaults which is likely Debug.
    [ValidateSet($null, 'Debug', 'Release')]
    [string[]] $Configuration = @(),

    # Indicates to include the binary logger which can be used with the MSBuild Structured Log Viewer.
    [Alias('BL')]
    [switch] $BinaryLogger,

    # Indicates to use Visual Studio Preview instead of released versions. Preview build must be installed in order to use.
    [switch] $VSPreview,

    # Indicates to execute the Pack target (generate NuGet packages).
    [switch] $Pack,

    # Indicates to execute the Clean target.
    [switch] $Clean,

    # Indicates to not add the Build target which is normally defaulted.
    [switch] $NoBuild,

    # Indicates to add the AppVeyor logger.
    [switch] $AppVeyorLogger
)

function Get-VSBuildFolder
{
    [OutputType([string])]
    param
    (
        [switch] $Prerelease
    )

    # https://github.com/Microsoft/vswhere
    $vsWhereExe = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    if(!(Test-Path -Path $vsWhereExe)) {
        throw "Unable to find vswhere, confirm Visual Studio is installed: $vsWhereExe"
    }

    $vsWhereArgs = @('-latest', '-format', 'json', '-requires', 'Microsoft.Component.MSBuild')
    if($Prerelease) {
        $vsWhereArgs += '-prerelease'
    }

    $vsWhereOutput = & $vsWhereExe $vsWhereArgs
    if(!$vsWhereOutput) {
        throw "Failed to get result from vswhere.exe"
    }

    $vsInstance = $vsWhereOutput | Out-String | ConvertFrom-Json | Select-Object -First 1
    Write-Verbose "Using VS instance: $($vsInstance.installationPath)"
    Write-Verbose "VS Version: $($vsInstance.installationVersion)"

    $msbuildPath = Join-Path -Path $vsInstance.installationPath -ChildPath 'MSBuild\Current\Bin\MSBuild.exe'
    if(!(Test-Path -Path $msbuildPath)) {
        throw "Unable to find MSBuild: $msbuildPath"
    }

    return $msbuildPath
}

$msbuildPath = Get-VSBuildFolder -Prerelease:$VSPreview

if(!$NoBuild) {
    $MSBuildTargets += 'Build'
}

if($Pack) {
    $MSBuildTargets += 'Pack'
}

if($Clean) {
    $MSBuildTargets += 'Clean'
}

$MSBuildTargets = $MSBuildTargets | Select-Object -Unique

$resolvedSolutionFile = (Resolve-Path -Path $Solution -ErrorAction Stop).ProviderPath
$msBuildArgs = @("`"$resolvedSolutionFile`"")
if($MSBuildTargets.Count -gt 0) {
    $msBuildArgs += ('/t:' + ($MSBuildTargets -join ','))
}

if($Configuration.Count -gt 0) {
    $MSBuildProperties['Configuration'] = $Configuration
}

if($MSBuildProperties.Count -gt 0) {
    $properties = @()
    foreach($property in $MSBuildProperties.GetEnumerator()) {
        $properties += "$($property.Key)=$(($property.Value -join ','))"
    }

    $msBuildArgs += ('/p:' + ($properties -join ';'))
}

if($AppVeyorLogger) {
    # https://www.appveyor.com/docs/build-phase/
    $msBuildArgs += '/logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"'
}

if($BinaryLogger) {
    $msBuildArgs += '/bl'
}

Write-Verbose "Executing: & $msbuildPath $($msBuildArgs -join ' ')"
& $msbuildPath $msBuildArgs

Write-Verbose "Build complete"