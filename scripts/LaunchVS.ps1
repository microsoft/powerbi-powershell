[CmdletBinding()]
param
(
    # Path to solution file. Defaults to <script dir>\..\dirs.proj.
    [ValidateNotNullOrEmpty()]
    [string] $Project = "$PSScriptRoot\..\dirs.proj",

    # Indicates to use Visual Studio Preview instead of released versions. Preview build must be installed in order to use.
    [switch] $VSPreview
)

Import-Module $PSScriptRoot\FindVS.psm1
$msbuildPath = Get-VSBuildFolder -Prerelease:$VSPreview

$Project = (Resolve-Path -Path $Project -ErrorAction Stop).ProviderPath

$msbuildArgs = @(
    '-restore',
    '-t:SlnGen',
    $Project
)

& $msbuildPath $msbuildArgs