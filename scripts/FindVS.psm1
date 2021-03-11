function Get-VSBuildFolder
{
    [OutputType([string])]
    param
    (
        [switch] $Prerelease
    )

    # https://github.com/Microsoft/vswhere
    $vsWhereExe = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    if(!(Test-Path -Path $vsWhereExe)) {
        throw "Unable to find vswhere, confirm Visual Studio is installed: $vsWhereExe"
    }

    $vsWhereArgs = @('-latest', '-requires', 'Microsoft.Component.MSBuild', '-find', 'MSBuild\**\Bin\MSBuild.exe')
    if($Prerelease) {
        $vsWhereArgs += '-prerelease'
    }

    # https://github.com/microsoft/vswhere/wiki/Find-MSBuild#powershell
    $msbuildPath = & $vsWhereExe $vsWhereArgs | select-object -first 1
    if(!(Test-Path -Path $msbuildPath)) {
        throw "Unable to find MSBuild: $msbuildPath"
    }

    return $msbuildPath
}