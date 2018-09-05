[CmdletBinding()]
[OutputType([string])]
param
(
    [ValidateNotNullOrEmpty()]
    [string] $SdkInstallDir = 'C:\Program Files\dotnet\sdk\'
)

# This script handles the missing netstandard.dll issue - https://github.com/PowerShell/PowerShell/blob/master/docs/cmdlet-example/command-line-simple-example.md#the-fix-for-missing-netstandarddll

# .NET CORE 2.0 Downloads - https://www.microsoft.com/net/download/dotnet-core/2.0
# .NET CORE 2.1 Downloads - https://www.microsoft.com/net/download/dotnet-core/2.1

$sdkDir = Get-ChildItem $SdkInstallDir -Directory | Where-Object Name -Match '\d\.\d\.\d' | ForEach-Object { 
    $version = [version]$_.BaseName
    $_ | Add-Member -Name SDKVersion -MemberType NoteProperty -Value $version
    $_ 
} | Sort-Object SDKVersion -Descending | Select-Object -First 1

Write-Verbose "Using SDK: $($sdkDir.FullName)"

$netStandardDllPath = Join-Path -Path $sdkDir.FullName -ChildPath 'Microsoft\Microsoft.NET.Build.Extensions\net461\lib\netstandard.dll'
if(Test-Path -Path $netStandardDllPath) {
    return $netStandardDllPath
}
else {
    throw "Unable to find netstandard assembly: $netStandardDllPath"
}