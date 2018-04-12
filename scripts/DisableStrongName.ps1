[CmdletBinding()]
param
(
    # Strong name keys to disable or enable (-EnableStrongName).
    [ValidateNotNull()]
    [string[]] $StrongNameKeys = @(
        '31bf3856ad364e35'
    ),

    # Indicates to enable strong name instead of disabling which is default behavior.
    [switch] $EnableStrongName
)

<#
# Purpose: This script is meant for disabling strong name validation.
# When building with Configuration=Release, the binaries are built with DelaySigned=true which causes strong name validation to take effect when loading.
#>

Write-Output "Running $($MyInvocation.MyCommand.Name)"
if (!([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([System.Security.Principal.WindowsBuiltInRole]::Administrator))
{
        throw "Run script as administrator"                                                                                                                                                                       
}

$x64Path = 'HKLM:\Software\Microsoft\StrongName\Verification\*,'
$x86Path = 'HKLM:\Software\Wow6432Node\Microsoft\StrongName\Verification\*,'

$StrongNameKeys = $StrongNameKeys | ForEach-Object {
    "$x64Path$_"
    "$x86Path$_"
}

foreach($strongNameKey in $StrongNameKeys) {
    if($EnableStrongName) {
        if((Test-Path -Path $strongNameKey)) {
            Write-Verbose "Removing key (enabling StrongName): $strongNameKey"
            Remove-Item $strongNameKey -Force -ErrorAction Stop
        }
    }
    else {
        if(!(Test-Path -Path $strongNameKey)) {
            Write-Verbose "Adding key (disabling StrongName): $strongNameKey"
            [void](New-Item $strongNameKey -Force -ErrorAction Stop)
        }
    }
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"