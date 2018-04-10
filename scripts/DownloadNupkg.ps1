[CmdletBinding()]
param
(
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectoryPath,

    [ValidateNotNullOrEmpty()]
    [string] $SourcePath,

    [ValidateNotnull()]
    [string[]] $PackageName
)

try {

    Write-Output "Installing the nuget package $PackageName from source $SourcePath to output directory $OutputDirectoryPath"

    nuget install $PackageName -OutputDirectory $OutputDirectoryPath -Source $SourcePath -DirectDownload  -PackageSaveMode nupkg
}
catch {
    Write-Host ($_ | Out-String) -ForegroundColor Red
    exit 1
}