##############################
#.SYNOPSIS
# Executes a command-line (CLI) build of the PowerBIPowerShell solution.
#
#.DESCRIPTION
# Invokes MSBuild by locating it and passing common properties and targets for the PowerBIPowerShell solution
#
#.EXAMPLE
# PS:> .\Build.ps1
# Executes just build for the solution.
#
#.EXAMPLE
# PS:> .\Build.ps1 -Pack -Clean
# Executes build, pack and clean for the solution.
#
#.NOTES
# Requires Visual Studio 2019 to be installed (at least 16.2 where vswhere.exe is available).
##############################
[CmdletBinding()]
param
(
    # Path to solution file. Defaults to <script dir>\..\dirs.proj.
    [ValidateNotNullOrEmpty()]
    [string] $Solution = "$PSScriptRoot\..\dirs.proj",

    # MSBuild targets to execute. Default is the Build target unless -NoBuild is specified.
    [ValidateNotNull()]
    [string[]] $MSBuildTargets = @(),

    # MSBuild properties to execute build with. Default is @{'GenerateFullPaths'='true'}.
    [ValidateNotNull()]
    [Hashtable] $MSBuildProperties = @{'GenerateFullPaths' = 'true' },

    # Build Configuration. Default is to use the MSBuild project defaults which is likely Debug.
    [ValidateSet($null, 'Debug', 'Release')]
    [string] $Configuration,

    # Indicates to include the binary logger which can be used with the MSBuild Structured Log Viewer.
    [Alias('BinLog', 'BL')]
    [switch] $BinaryLogger,

    # Indicates to use Visual Studio Preview instead of released versions. Preview build must be installed in order to use.
    [switch] $VSPreview,

    # Indicates to execute the Pack target (generate NuGet packages).
    [switch] $Pack,

    # Indicates to execute the Clean target. -Restore is added if you specify this.
    [switch] $Clean,

    # Indicates to nuget restore.
    [switch] $Restore,

    # Indicates to not add the Build target which is normally defaulted.
    [switch] $NoBuild,

    # Indicates to add the AppVeyor logger.
    [switch] $AppVeyorLogger,

    # Indicates to not build in parallel, removes the /m switch.
    [switch] $NoParallel,

    # Indicates to force the use certain type of build.
    [ValidateSet('msbuild', 'dotnet')]
    [string] $BuildMode
)

function InvokeMSBuild {
    try {
        Import-Module $PSScriptRoot\FindVS.psm1
        $msbuildPath = Get-VSBuildFolder -Prerelease:($VSPreview.IsPresent)
    }
    catch {
        if ($BuildMode -eq 'msbuild') {
            Write-Error "Failed to locate MSBuild. Ensure Visual Studio 2022 is installed and VS Dev Console loaded and try again." -ErrorAction Continue
            Write-Error $_ -ErrorAction Continue
            exit 1
        }
        else {
            Write-Warning "Failed to locate MSBuild. Falling back to dotnet build."
            InvokeDotNetBuild
            return
        }
    }

    if (!$NoBuild) {
        $MSBuildTargets += @('Build')
    }

    if ($Pack) {
        $MSBuildTargets += @('Pack')
    }

    if ($Clean) {
        $MSBuildTargets += @('Clean')
    }

    $MSBuildTargets = $MSBuildTargets | Select-Object -Unique

    $resolvedSolutionFile = (Resolve-Path -Path $Solution -ErrorAction Stop).ProviderPath
    $msBuildArgs = @("$resolvedSolutionFile")
    if ($MSBuildTargets.Count -gt 0) {
        $msBuildArgs += ('/t:' + ($MSBuildTargets -join ','))
    }

    if ($Configuration) {
        $MSBuildProperties['Configuration'] = $Configuration
    }

    if ($MSBuildProperties.Count -gt 0) {
        $properties = @()
        foreach ($property in $MSBuildProperties.GetEnumerator()) {
            $properties += "$($property.Key)=$(($property.Value -join ','))"
        }

        $msBuildArgs += ('/p:' + ($properties -join ';'))
    }

    if ($Restore -or $Clean) {
        $msBuildArgs += '/restore'
    }

    if ($AppVeyorLogger) {
        # https://www.appveyor.com/docs/build-phase/
        $msBuildArgs += '/logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"'
    }

    if ($BinaryLogger) {
        $msBuildArgs += '/bl'
    }

    if (!$NoParallel) {
        $msBuildArgs += '/m'
    }

    Write-Host "Executing: & $msbuildPath $($msBuildArgs -join ' ')" -ForegroundColor Magenta
    $msBuildArgs | Out-Host

    & $msbuildPath $msBuildArgs
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

function InvokeDotNetBuild {
    if (!(Get-Command dotnet -ErrorAction SilentlyContinue)) {
        Write-Error "dotnet not found. Ensure the .NET Core SDK is installed and available in the PATH." -ErrorAction Continue
        exit 1
    }

    $fullSolutionPath = (Resolve-Path -Path $Solution -ErrorAction Stop).ProviderPath

    # https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build
    $dotnetArgs = @()
    if ($Pack -or $NoBuild) {
        $dotnetArgs += 'pack'
    }
    else {
        $dotnetArgs += 'build'
    }

    if ($Configuration) {
        $dotnetArgs += '-c'
        $dotnetArgs += $Configuration
    }

    if ($BinaryLogger) {
        $msBuildArgs += '-bl'
    }

    if ($NoBuild) {
        $dotnetArgs += '--no-build'
    }

    if (!$Restore) {
        $dotnetArgs += '--no-restore'
    }

    if ($Clean) {
        #$dotnetArgs += '--no-incremental'
        Write-Host "Calling: dotnet clean $fullSolutionPath" -ForegroundColor Magenta
        & dotnet clean $fullSolutionPath
    }

    if ($MSBuildProperties -and ($MSBuildProperties.Count -gt 0)) {
        foreach ($property in $MSBuildProperties.GetEnumerator()) {
            $dotnetArgs += "--property:$($property.Key)=$(($property.Value -join ','))"
        }
    }
    
    $dotnetArgs += $fullSolutionPath
    
    Write-Host "Calling: dotnet $($dotnetArgs -join ' ')" -ForegroundColor Magenta
    & dotnet $dotnetArgs
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

if ($IsLinux -or $IsMacOS) {
    Write-Verbose "Linux or MacOS detected, forcing DotNet build"
    $BuildMode = 'dotnet'
}


if ($BuildMode -eq 'dotnet') {
    InvokeDotNetBuild
}
else {
    Write-Verbose "Windows detected, checking for MSBuild"
    InvokeMSBuild
}

Write-Verbose "Build complete"