##############################
#.SYNOPSIS
# Updates module dependencies in *.nupsec and *.psd1 files.
#
#.DESCRIPTION
# Updates .NET Standard 2.0 dependencies to be generic and writes to *.nuspec.
# Uses *.nuspec to write RequiresModules into *.psd1 (module manifest).
#
#.EXAMPLE
# PS:> .\UpdateModuleDependencies.ps1 -Path ..\PkgOut\Reset
# Updates any *.nuspec and *.psd1 files under directory ..\PkgOut\Reset
#
##############################
[CmdletBinding()]
param
(
    # Path to locate *.nuspec and *.psd1 files under to update.
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $Path
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"

if(!(Test-Path -Path $Path)) {
    throw "-Path not found: $Path"
}

$nuspecFiles = Get-Childitem -Path $Path -Recurse -Filter *.nuspec -Depth 3
if(!$nuspecFiles) {
    throw "Failed to find *.nuspec files under: $Path"
}

foreach($nuspecFile in $nuspecFiles) {
    $nuspecPath = $nuspecFile.FullName
    Write-Output "Processing: $nuspecPath"
    
    $moduleDir = $nuspecFile.Directory.FullName
    $psdFilePath = Get-Childitem -Path $moduleDir -Filter *.psd1 | Select-Object -First 1 | ForEach-Object { $_.FullName }
    if(!$psdFilePath) {
        throw "Unable to find the module file under path: $moduleDir"
    }
    
    Write-Verbose "The path to the module file: $psdFilePath"

    Write-Verbose "Getting content for nuspec from $nuspecPath"
    $nuspecContent = [xml](Get-Content -Path $nuspecPath -Raw -ErrorAction Stop)
    if(!$nuspecContent) {
        throw "Nuspec file is empty under path: $nuspecPath"
    }

    Write-Verbose "Found content for nuspec file"

    Write-Output "Updating dependencies..."
    Write-Verbose "Get all dependencies under .NET Standard 2.0 Group"
    $dep = $nuspecContent.package.metadata.dependencies.group.dependency

    Write-Verbose "Removing the attributes which are not required and appending the dependencies"
    $dep | ForEach-Object { 
        if($null -eq $dep) { return }

        $_.RemoveAttribute("exclude", $nuspec.package.xmlns); 
        $nuspecContent.package.metadata.dependencies.AppendChild($_) 
    } | Out-Null

    Write-Verbose "Removing the .NET Standard 2.0 Group from dependencies"
    [void]($nuspecContent.package.metadata.dependencies.RemoveChild($nuspecContent.package.metadata.dependencies.group))

    Write-Output "Saving nuspec..."
    Write-Verbose "Updating the nuspec content with the changes under: $nuspecPath"
    $nuspecContent.Save($nuspecPath)

    Write-Output "Processing: $psdFilePath"
    $psdFileContent = Get-Content -Path $psdFilePath -Raw -ErrorAction Stop
    if(!$psdFileContent) {
        throw "Module file is empty under path: $psdFilePath"
    }

    Write-Verbose "Found content for module file"
    $updatePsdFile = $false

    Write-Verbose "Getting list of all dependencies to add to module file Required Modules section"
    if($dep) {
        $depIds = ($dep).id | Foreach-Object { "'$_'" }
        $dependencies = $depIds -join ', '
        Write-Verbose "Dependency Ids: $dependencies"

        Write-Output "Updating Required Modules section..."
        $replaceRequiredModules = " RequiredModules = @($dependencies)"
        $psdFileContent = $psdFileContent -replace '# RequiredModules = @[(][)]', $replaceRequiredModules
        $updatePsdFile = $true
    }

    if($updatePsdFile) {
        Write-Output "Updating module file..."
        $utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($false)
        [System.IO.File]::WriteAllText($psdFilePath, $psdFileContent, $utf8NoBomEncoding)
    }
    else
    {
        Write-Warning "No updates made to module file: $psdFilePath"
    }
}

Write-Output "Completed running $($MyInvocation.MyCommand.Name)"