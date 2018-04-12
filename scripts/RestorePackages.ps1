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