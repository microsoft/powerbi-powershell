[CmdletBinding()]
param
(
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectory,

    [ValidateNotNullOrEmpty()]
    [string] $NugetSource = "https://ci.appveyor.com/nuget/powerbi-powershell-j0f50attoqd6",

    [string[]] $PackageNames = @('MicrosoftPowerBIMgmt', 'MicrosoftPowerBIMgmt.Profile', 'MicrosoftPowerBIMgmt.Workspaces', 'MicrosoftPowerBIMgmt.Reports'),

    [string] $PackageVersion,

    [switch] $Prerelease
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"
$nugetExe = Get-Command 'nuget.exe' -ErrorAction Stop

if(!(Test-Path -Path  $OutputDirectory)) {
    [void](New-Item -Path $OutputDirectory -ItemType Directory -Force)
}

if(!$PackageNames -or $PackageNames.Count -lt 1) {
    Write-Output "Determining package names..."
    $nugetArgs = @('list', '-Source', $NugetSource, '-ForceEnglishOutput', '-NonInteractive')
    if($PackageVersion) {
        $nugetArgs += '-AllVersions'
    }

    if($Prerelease) {
        $nugetArgs += '-Prerelease'
    }

    Write-Verbose "Running: & $nugetExe $($nugetArgs -join ' ')"
    $nugetPackages = & $nugetExe $nugetArgs
    if($LASTEXITCODE -ne 0) {
        throw "Nuget.exe failed with exit code: $LASTEXITCODE"
    }

    $groupedPackgesByVersion = $nugetPackages | ForEach-Object { $t = $_ -split ' '; [pscustomobject]@{Name=$t[0]; Version=$t[1] }} | Group-Object -Property Version -AsHashTable
    if($PackageVersion) {
        $PackageNames = ($groupedPackgesByVersion[$PackageVersion]).Name
    }
    else {
        $PackageNames = ($groupedPackgesByVersion.Values).Name
    }
}

Write-Output "Installing the nuget package(s) '$($PackageNames -join ', ')' from source '$NugetSource' to output directory '$OutputDirectory'"

foreach($packageName in $PackageNames) {
    Write-Output "Getting package: $packageName"
    $nugetArgs = @('install', $packageName, '-OutputDirectory', $OutputDirectory, '-Source', $NugetSource, '-PackageSaveMode', 'nuspec', '-NonInteractive')
    
    if($PackageVersion) {
        $nugetArgs += @('-Version', $PackageVersion)
    }
    
    if($Prerelease) {
        $nugetArgs += '-Prerelease'
    }

    Write-Verbose "Running: & $nugetExe $($nugetArgs -join ' ')"
    & $nugetExe $nugetArgs
    if($LASTEXITCODE -ne 0) {
        throw "Nuget.exe failed with exit code: $LASTEXITCODE"
    }

    # $nugetPackageDirectory = Get-ChildItem -Path $OutputDirectory | Where-Object { $_.Name -match "$([regex]::Escape($packageName)).\d+.*" }
    # if(!$nugetPackageDirectory) {
    #     throw "Failed to locate package '$packageName' in directory: $OutputDirectory"
    # }

    # Get-ChildItem -Path $nugetPackageDirectory.FullName -Exclude *.nupkg | Remove-Item -Recurse -Force
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"