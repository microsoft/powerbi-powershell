$tempDir = [IO.Path]::GetTempPath()
if ($IsWindows) {
    $scriptPath = Join-Path -Path $tempDir -ChildPath 'dotnet-install.ps1'
    if (Test-Path -Path $scriptPath) {
        Remove-Item -Path $scriptPath -Force
    }

    Write-Host 'Downloading https://dot.net/v1/dotnet-install.ps1'
    Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile $scriptPath
    Unblock-File $scriptPath
}
elseif ($IsLinux -or $IsMacOS) {
    # Bash scripts work on both OS and are stored in the Linux directory
    # wget -q "https://dot.net/v1/dotnet-install.sh"
    $scriptPath = Join-Path -Path $tempDir -ChildPath 'dotnet-install.sh'
    if (Test-Path -Path $scriptPath) {
        Remove-Item -Path $scriptPath -Force
    }

    Write-Host 'Downloading https://dot.net/v1/dotnet-install.sh'
    Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.sh' -OutFile $scriptPath
    chmod +x $scriptPath
}

$globalJsonPath = (Resolve-Path "$PSScriptRoot/../global.json" -ErrorAction Stop).ProviderPath

# For arguments to dotnet script see:
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script

if ($IsWindows) {
    Write-Host "Executing: $scriptPath -JSonFile $globalJsonPath"
    & $scriptPath -JSonFile $globalJsonPath
}
elseif ($IsLinux -or $IsMacOS) {
    Write-Host "Executing: $scriptPath --jsonfile `"$globalJsonPath`""
    & $scriptPath --jsonfile "$globalJsonPath"
}
else {
    Write-Error "Unsupported OS platform" -ErrorAction Continue
    exit 1
}