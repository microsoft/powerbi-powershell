[CmdletBinding(SupportsShouldProcess)]
param
(
    [switch] $Build,

    [ValidateNotNull()]
    [hashtable] $BuildScriptArgs = @{'Pack'=$true; 'Clean'=$true; 'Verbose'=$true},

    [string] $PackageDir = "$PSScriptRoot\..\PkgOut",

    [switch] $Force
)

if($Build) {
    & "$PSScriptRoot\Build.ps1" @BuildScriptArgs
    if($LASTEXITCODE -ne 0) {
        throw "MSBuild failed with exit code $LASTEXITCODE"
    }
}

if(!(Test-Path -Path $PackageDir)) {
    throw "Directory '$PackageDir' does not exist, specify -Build to generate"
}

$nupkgFiles = Get-ChildItem -Path $PackageDir -Filter *.nupkg
if(!$nupkgFiles) {
    throw "No NUPKG files found, specify -Build to generate"
}

$nonNuPkgFiles = Get-ChildItem -Path $PackageDir -Exclude *.nupkg
if($nonNuPkgFiles) {
    if(!$Force -and !$PSCmdlet.ShouldContinue("$($nonNuPkgFiles | ForEach-Object { $_.FullName } | Out-String)", "Delete files")) {
        exit 0
    }

    Write-Verbose "Removing non-NUPKG files..."
    $nonNuPkgFiles | Remove-Item -Force -ErrorAction Stop -Recurse
}

$PackageDir = (Resolve-Path -Path $PackageDir).ProviderPath
Add-Type -AssemblyName System.IO.Compression.FileSystem

$nupkgFiles | ForEach-Object {
    $moduleName = $_.BaseName -split '\.\d' | Select-Object -First 1
    Write-Verbose "Unpacking zip '$($_.Name)' to directory '$moduleName'..."
    $newName = Rename-Item -Path $_.FullName -NewName "$moduleName.zip" -Force -PassThru
    $outDir = Join-Path -Path $PackageDir -ChildPath $moduleName
    [System.IO.Compression.ZipFile]::ExtractToDirectory($newName.FullName, $outDir)
}

if($env:PSModulePath -split ';' -inotcontains $PackageDir) {
    Write-Verbose "Adding '$PackageDir' to PSModulePath environment variable..."
    $env:PSModulePath = $env:PSModulePath.TrimEnd(';')
    $env:PSModulePath += ";$PackageDir"
}

Write-Output "[Done] Updated PSModulePath with '$PackageDir', you can now invoke cmdlets (causes module auto-loading)"