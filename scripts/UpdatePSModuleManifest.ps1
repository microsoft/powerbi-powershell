##############################
#.SYNOPSIS
# Updates a PowerShell manifest (*.psd1) file.
#
#.DESCRIPTION
# Updates PowerShell manifest (*.psd1) content such as Version, release notes, root module.
#
#.EXAMPLE
# PS:> .\UpdatePSModuleManifest.ps1 -ModulePath ..\src\Modules\Profile\Commands.Profile\bin\Debug\netstandard2.0\MicrosoftPowerBIMgmt.Profile.psd1 -TargetFramework netstandard2.0 -Version 1.1.0
# Updates content in *.psd1 file specified to have version 1.1.0 and target path in RootModule.
#
##############################
[CmdletBinding()]
param
(
    # Path to PowerShell manifest (*.psd1) file to update.
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $ModulePath,

    # Target framework to append to RootModule.
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $TargetFramework,

    # Version of the module.
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $Version,

    # Release notes of the module.
    [string] $ReleaseNotes
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"
$psdFileContent = Get-Content -Path $ModulePath -Raw -ErrorAction Stop
$updatePsdFile = $false

# Update RootModule
$matchRegex = "(?<Front>RootModule = ')(?<DllName>.+)(?<Back>')"
$addTargetFramework = ".\lib\$TargetFramework\"
$replaceMatch = '${Front}' + $addTargetFramework + '${DllName}${Back}'

if(($psdFileContent -match $matchRegex) -and (!($Matches['DllName'].StartsWith($addTargetFramework)))) {
    Write-Output "Adding '$addTargetFramework' to RootModule: $ModulePath"
    $psdFileContent = $psdFileContent -replace $matchRegex, $replaceMatch
    $updatePsdFile = $true
}
else {
    Write-Output "Module '$ModulePath' already contains target framework"
}

# Update ModuleVersion
$matchRegex = "(?<Front>ModuleVersion = ')(?<ModVersion>.+)(?<Back>')"
$replaceMatch = '${Front}' + $Version + '${Back}'

if(($psdFileContent -match $matchRegex) -and ($Matches['ModVersion'] -ne $Version)) {
    Write-Output "Updating ModuleVersion to: $Version"
    $psdFileContent = $psdFileContent -replace $matchRegex, $replaceMatch
    $updatePsdFile = $true
}
else {
    Write-Output "Module '$ModulePath' already contains ModuleVersion $($Matches['ModVersion'])"
}

# Add release notes
if($ReleaseNotes) {
    Write-Output "Adding Release notes"
	if(Test-Path -Path $ReleaseNotes) {
		$ReleaseNotes = Get-Content -Path $ReleaseNotes | Out-String
		$ReleaseNotes = $ReleaseNotes.Trim()
	}

    $replaceReleaseNotesText = @"
        ReleaseNotes = @'
$ReleaseNotes
'@
"@
    $psdFileContent = $psdFileContent -replace "# ReleaseNotes = ''", $replaceReleaseNotesText
    $updatePsdFile = $true
}

if($updatePsdFile) {
    Write-Output "Updating module file: $ModulePath"
    $utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($false)
    [System.IO.File]::WriteAllText($ModulePath, $psdFileContent, $utf8NoBomEncoding)
}
else {
    Write-Warning "No updates made to module file: $ModulePath"
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"