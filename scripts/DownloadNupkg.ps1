[CmdletBinding()]
param
(
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectory,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $SourcePath,

    [Parameter(Mandatory)]
    [ValidateNotnull()]
    [string[]] $PackageName
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"
$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop

if(!(Test-Path -Path  $OutputDirectory)) {
    [void](New-Item -Path $OutputDirectory -ItemType Directory -Force)
}

Write-Output "Installing the nuget package(s) '$($PackageName -join ', ')' from source '$SourcePath' to output directory '$OutputDirectory'"

$nugetArgs = @('install', $PackageName, '-OutputDirectory', $OutputDirectory, '-Source', $SourcePath, '-DirectDownload', '-PackageSaveMode', 'nupkg')
Write-Verbose "Running: & $nugetExe $($nugetArgs -join ' ')"
& $nugetExe $nugetArgs
if($LASTEXITCODE -ne 0) {
    throw "Nuget.exe failed with exit code: $LASTEXITCODE"
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"