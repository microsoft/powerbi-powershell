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
# Requires Visual Studio 2019 to be installed (at least 16.2 where vswhere.exe is available).
##############################
[CmdletBinding()]
param
(
    # Path to solution file. Defaults to <script dir>\..\dirs.proj.
    [ValidateNotNullOrEmpty()]
    [string] $Solution = "$PSScriptRoot\..\dirs.proj",

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
    [switch] $AppVeyorLogger,

    # Indicates to not build in parallel, removes the /m switch.
    [switch] $NoParallel
)

Import-Module $PSScriptRoot\FindVS.psm1
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

if(!$NoParallel) {
    $msBuildArgs += '/m'
}

Write-Verbose "Executing: & $msbuildPath $($msBuildArgs -join ' ')"
& $msbuildPath $msBuildArgs

Write-Verbose "Build complete"