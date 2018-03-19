[CmdletBinding()]
param
(
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $ModulePath,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $TargetFramework,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $Version,

    [switch] $RemovePrereleaseTag,

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

# Remove Prelease tag
if($RemovePrereleaseTag) {
    Write-Output "Removing Prerelease tag"
    $psdFileContent = $psdFileContent -replace "Prerelease = '.*'", ''
    $updatePsdFile = $true
}

# Add release notes
if($ReleaseNotes) {
    Write-Output "Adding Release notes"
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