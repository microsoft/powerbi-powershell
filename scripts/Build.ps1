[CmdletBinding()]
param
(
    [switch] $VSPreview,

    [switch] $Pack,

    [switch] $Clean,

    [switch] $NoBuild,

    [string] $Solution = "$PSScriptRoot\..\src\PowerBIPowerShell.sln",

    [ValidateNotNull()]
    [string[]] $MSBuildTargets = @(),

    [ValidateNotNull()]
    [Hashtable] $MSBuildProperties = @{},

    [Alias('BL')]
    [switch] $BinaryLogger
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

    $vsInstance = $vsWhereOutput | ConvertFrom-Json | Select-Object -First 1
    Write-Verbose "Using VS instance: $($vsInstance.installationPath)"
    Write-Verbose "VS Version: $($vsInstance.installationVersion)"

    $msbuildPath = Join-Path -Path $vsInstance.installationPath -ChildPath 'MSBuild\15.0\Bin\MSBuild.exe'
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

if($MSBuildProperties.Count -gt 0) {
    $properties = @()
    foreach($property in $MSBuildProperties.GetEnumerator()) {
        $properties += "$($property.Key)=$($property.Value)"
    }

    $msBuildArgs += ('/t:' + ($properties -join ';'))
}

if($BinaryLogger) {
    $msBuildArgs += '/bl'
}

Write-Verbose "Executing: & $msbuildPath $($msBuildArgs -join ' ')"
& $msbuildPath $msBuildArgs

Write-Verbose "Build complete"