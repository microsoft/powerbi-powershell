##############################
#.SYNOPSIS
# Restores NuGet packages (*.nupkg) for a PowerBIPowerShell solution.
#
#.DESCRIPTION
# Executes nuget.exe against the PowerBIPowerShell solution to restore any NuGet package dependencies the solution contains.
#
#.EXAMPLE
# PS:> .\RestorePackages.ps1
# Runs NuGet Restore against PowerBIPowerShell solution file.
#
#.NOTES
# This script requires nuget.exe to be available in your $env:Path.
##############################
[CmdletBinding()]
param
(
    # Path to solution file. Defaults to <script dir>\..\dirs.proj.
    [ValidateNotNullOrEmpty()]
    [string] $Solution = "$PSScriptRoot\..\dirs.proj",

    # Indicates to use Visual Studio Preview instead of released versions. Preview build must be installed in order to use.
    [switch] $VSPreview
)

Import-Module $PSScriptRoot\FindVS.psm1
$msbuildPath = Get-VSBuildFolder -Prerelease:$VSPreview

$Solution = (Resolve-Path -Path $Solution -ErrorAction Stop).ProviderPath

$msbuildArgs = @(
    '-restore',
    '-m',
    '-t:Restore',
    '-graph:true',
    $Solution
)

& $msbuildPath $msbuildArgs

if ($LastExitCode -ne 0) {
    throw "Failed to restore nuget packages: $LastExitCode"
}