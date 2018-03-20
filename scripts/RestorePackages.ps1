[CmdletBinding()]
param
(
    [ValidateNotNullOrEmpty()]
    [string] $Solution = "$PSScriptRoot\..\src\PowerBIPowerShell.sln"
)

$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop
$Solution = (Resolve-Path -Path $Solution -ErrorAction Stop).ProviderPath

& $nugetExe restore $Solution