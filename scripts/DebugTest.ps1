##############################
#.SYNOPSIS
# Executes .\Build.ps1 and .\Test.ps1 for use in debugging unit tests in Visual Studio Code.
#
#.DESCRIPTION
# Invokes MSBuild and dotnet test.
# Instructs 'dotnet test' to execute a specific test name under a test project (via filter) and allows a debugger to attach to dotnet.exe.
# Meant to be called from Visual Studio Code, see ..\.vscode\tasks.json.
#
#.EXAMPLE
# PS:> .\DebugTest.ps1 -TestName 'UnitTestName' -DirectoryName 'c:\pbi-ps\src\Modules\Profile\Commands.Profile.Test'
# Executes a build followed by 'dotnet test' supplying testname and *.csproj located under the directory name to '--filter'.
#
#.NOTES
# Requires Visual Studio 2017 to be installed (at least 15.2 where vswhere.exe is available).
##############################
param
(
    # Name of unit test to execute.
    [string] $TestName,

    # Directory containing *.csproj for unit test.
    [string] $DirectoryName,

    # Indicates to not build before executing unit tests.
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

# Run tests
Write-Host "Set breakpoint in test method '$TestName', and use .NET Core Attach to debug test (using process ID)" -ForegroundColor Magenta
& "$PSScriptRoot\Test.ps1" -VSTestHostDebug -TestName $TestName -TestProject $csProj.BaseName -Filter '' # Set filter to empty string to allow interactive tests to be debugged