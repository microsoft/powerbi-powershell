[CmdletBinding()]
param
(
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $Path
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"

$nugetPackages = Get-Childitem –Path $Path

foreach($nugetPackage in $nugetPackages)
{
    $nugetPackagePath = Join-Path $Path $nugetPackage.Name

    $nuspecPath = (Get-Childitem –Path $nugetPackagePath -Filter *.nuspec -Recurse).FullName
    if(!$nuspecPaths)
    {
        throw "Unable to find the nuspec under path: $nugetPackagePath"
    }
    Write-Output "The path to nuspec: $nuspecPath"

    $psdFilePath = (Get-Childitem –Path $nugetPackagePath -Filter *.psd1 -Recurse).FullName
    if(!$psdFilePath)
    {
        throw "Unable to find the module file under path: $nugetPackagePath"
    }
    Write-Output "The path to the module file: $psdFilePath"

    Write-Output "Getting content for nuspec from $nuspecPath"
    $nuspecContent = [xml](Get-Content -Path $nuspecPath -Raw)
    if(!$nuspecContent)
    {
        throw "Nuspec file is empty under path: $nuspecPath"
    }
    Write-Output "Found content for nuspec file"

    Write-Output "Removing the xmlns attribute from package"
    $nuspec.package.RemoveAttribute("xmlns")

    Write-Output "Get all dependencies under .NET Standard 2.0 Group"
    $dep = $nuspecContent.package.metadata.dependencies.group.dependency

    Write-Output "Removing the attributes which are not required and appending the dependencies"
    if ($dep)
    {
        $dep | % { $_.RemoveAttribute("exclude"); $nuspecContent.package.metadata.dependencies.AppendChild($_) }
    }

    Write-Output "Removing the .NET Standard 2.0 Group from dependencies"
    $nuspecContent.package.metadata.dependencies.RemoveChild($nuspecContent.package.metadata.dependencies.group)

    Write-Output "Updating the nuspec content with the changes under: $nuspecPath"
    $nuspecContent.Save($nuspecPath)

    Write-Output "Getting content for module file from $psdFilePath"
    $psdFileContent = Get-Content -Path $psdFilePath -Raw -ErrorAction Stop
    if(!$psdFileContent)
    {
        throw "Module file is empty under path: $psdFilePath"
    }
    Write-Output "Found content for module file"
    $updatePsdFile = $false

    Write-Output "Getting list of all dependencies to add to module file Required Modules section"
    if($dep)
    {
        $depIds = ($dep).id | Foreach-Object { "'$_'" }
        $dependencies = $depIds -join ', '
        Write-Output "Join result is $dependencies"

        Write-Output "Adding the Required Modules section"
        $replaceRequiredModules = " RequiredModules = @($dependencies)"
        $psdFileContent = $psdFileContent -replace '# RequiredModules = @[(][)]', $replaceRequiredModules
        $updatePsdFile = $true
    }

    if($updatePsdFile) 
    {
        Write-Output "Updating module file: $psdFilePath"
        $utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($false)
        [System.IO.File]::WriteAllText($psdFilePath, $psdFileContent, $utf8NoBomEncoding)
    }
    else 
    {
        Write-Warning "No updates made to module file: $psdFilePath"
    }
}
