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
    # Path to solution file. Defaults to <script dir>\src\PowerBIPowerShell.sln.
    [ValidateNotNullOrEmpty()]
    [string] $Solution = "$PSScriptRoot\..\src\PowerBIPowerShell.sln"
)

$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop
$Solution = (Resolve-Path -Path $Solution -ErrorAction Stop).ProviderPath

& $nugetExe restore $Solution

if ($LastExitCode -ne 0) {
    throw "Failed to restore nuget packages: $LastExitCode"
}
