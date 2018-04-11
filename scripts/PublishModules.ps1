[CmdletBinding(SupportsShouldProcess=$true)]
param
(
    [ValidateNotNullOrEmpty()]
    [string] $Path,

    [ValidateNotNullOrEmpty()]
    [string] $ApiKey,

    [ValidateNotNullOrEmpty()]
    [string] $NuGetSource = 'https://www.powershellgallery.com/api/v2/'
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"

if(!(Test-Path -Path $Path)) {
    throw "-Path not found: $Path"
}

$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop

$nupkgFiles = Get-ChildItem -Path $Path -Filter *.nupkg -Recurse
if(!$nupkgFiles) {
    throw "No *.nupkg files found under: $Path"
}

Write-Output "Nuget packages to Publish:`n$($nupkgFiles | ForEach-Object { $_.FullName } | Out-String)"
foreach($nupkgFile in $nupkgFiles) {
    $nugetFilePath = $nupkgFile.FullName
    $nugetPackageName = $nupkgFile.Name

    $nugetArgs = @('push', "$nugetFilePath", '-Source', $NuGetSource, '-ApiKey', $ApiKey, '-NonInteractive')
    
    if($PSCmdlet.ShouldProcess($nupkgFile, "Publish NuGet Package")) {
        Write-Output "Publishing package: $nugetFilePath"
        & $nugetExe $nugetArgs
        if($LASTEXITCODE -ne 0) {
            throw "Failed to publish package $nugetPackageName"
        }

        Write-Output "Pushed package $nugetPackageName to $NuGetSource"
    }
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"