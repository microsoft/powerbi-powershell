param
(
    [string] $TestName,
    [string] $DirectoryName,
    [switch] $NoBuild
)

# https://code.visualstudio.com/docs/editor/debugging
# https://code.visualstudio.com/docs/editor/variables-reference

if(!$TestName) {
    throw "Select test name in VSCode before running!"
}

if(!$DirectoryName) {
    throw "Failed to get directory name"
}

Write-Host "TestName: $TestName"
Write-Host "DirectoryName: $DirectoryName"

# Build
if(!$NoBuild) {
    & "$PSScriptRoot\Build.ps1"
}

# Locate test assembly
$csProj = Get-ChildItem -Path $DirectoryName -Filter '*.csproj' | Select-Object -First 1
if(!$csProj) {
    throw "Failed to find CSProj in: $DirectoryName"
}

# [xml] $csProjXml = Get-Content -Path $csProj.FullName -Raw
# $pG = $csProjXml.Project.PropertyGroup | Select-Object -First 1
# $assemblyName = $pG.AssemblyName
# if(!$assemblyName) {
#     throw "Failed to get assembly name from CSProj: $($csProj.FullName)"
# }

# Run tests
Write-Host "Set breakpoint in test method '$TestName', and use .NET Core Attach to debug test (using process ID)" -ForegroundColor Magenta
& "$PSScriptRoot\Test.ps1" -VSTestHostDebug -TestName $TestName -TestProject $csProj.BaseName -Filter '' # Set filter to empty string to allow interactive tests to be debugged