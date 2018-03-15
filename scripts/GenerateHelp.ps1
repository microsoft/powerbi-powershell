[CmdletBinding()]
param
(
    [Parameter(Mandatory, ParameterSetName="GenerateMarkdown")]
    [string] $ModulePath,

    [string] $PathToPlatyPS,

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $ProjectRoot,

    [Parameter(Mandatory, ParameterSetName="GenerateExternalHelp")]
    [string] $OutputPath,

    [ValidateNotNullOrEmpty()]
    [string] $Language = 'en-US',

    [string] $TargetFramework,

    [Parameter(Mandatory, ParameterSetName="GenerateMarkdown")]
    [switch] $GenerateMarkdown,

    [Parameter(Mandatory, ParameterSetName="GenerateExternalHelp")]
    [switch] $GenerateExternalHelp,

    [string] $DependentAssemblies
)

Write-Output "Running $($MyInvocation.MyCommand.Name)"

if(!(Test-Path -Path $ProjectRoot)) {
    throw "Unable to find -ProjectRoot: $ProjectRoot"
}

if($PathToPlatyPS) {
    $platyPSModulePath = Join-Path -Path $PathToPlatyPS -ChildPath 'platyPS.psd1'
    if(!(Test-Path -Path $platyPSModulePath)) {
        throw "Unable to find -PathToPlatyPS: $platyPSModulePath"
    }
    Write-Output "Loading platyPS from: $platyPSModulePath"
    Import-Module -Name $platyPSModulePath -ErrorAction Stop -Force
}
else {
    Write-Output "Loading platyPS from installed modules"
    Import-Module -Name 'platyPS' -ErrorAction Stop
}

$helpFolder = Join-Path -Path $ProjectRoot -ChildPath 'help'

if($GenerateMarkdown) {
    if(!(Test-Path -Path $ModulePath)) {
        throw "Unable to find -ModulePath: $ModulePath"
    }

    if($DependentAssemblies) {
        [string[]]$assemblyPaths = $DependentAssemblies -split ';'
        $assemblyLookup = @{}
        foreach($assemblyPath in $assemblyPaths) {
            $assemblyItem = Get-Item -Path $assemblyPath -ErrorAction SilentlyContinue
            if($assemblyItem) {
                $assemblyLookup[$assemblyItem.BaseName] = $assemblyItem.FullName
            }
        }

        $onAssemblyResolve = [System.ResolveEventHandler] {
            param($sender, $e)
    
            try {
                $assemblyName = (New-Object System.Reflection.AssemblyName($e.Name)).Name
                if(!$assemblyName) {
                    return $null
                }
    
                if(($assemblyName.EndsWith('.resources')) -or ($assemblyName.StartsWith("Microsoft.PowerShell.")) -or ($assemblyName.StartsWith("Microsoft.WSMan."))) {
                    return $null
                }

                if($assemblyLookup.ContainsKey($assemblyName)) {
                    return [System.Reflection.Assembly]::LoadFile($assemblyLookup[$assemblyName])
                }
            }
            catch {
                # Ignore
            }
    
            return $null
        }

        [System.AppDomain]::CurrentDomain.add_AssemblyResolve($onAssemblyResolve)
    }

    $moduleFile = Get-Item -Path $ModulePath
    Write-Output "Loading PSD1 module: $ModulePath"
    Import-Module -Name $ModulePath -ErrorAction Stop -Force -Global
    $moduleName = $moduleFile.BaseName

    if(!(Test-Path -Path $helpFolder)) {
        Write-Output "Creating folder: $helpFolder"
        [void](New-Item -Path $helpFolder -ItemType Directory -ErrorAction SilentlyContinue -Force)
    }

    if(Get-ChildItem -Path $helpFolder -Filter *.md) {
        Write-Output "Updating markdown files: $helpFolder"
        [void](Update-MarkdownHelpModule -Path $helpFolder -RefreshModulePage -AlphabeticParamsOrder -ErrorAction Stop)
    }
    else {
        Write-Output "Creating new Markdown files for module '$moduleName': $helpFolder"
        [void](New-MarkdownHelp -Module $moduleName -Locale $Language -OutputFolder $helpFolder `
                        -WithModulePage -AlphabeticParamsOrder -ErrorAction Stop)
    }

    Write-Output "Finished generating Markdown files"
    Remove-Module -Name $moduleName -Force -ErrorAction SilentlyContinue
}

if($GenerateExternalHelp) {
    if(!(Test-Path -Path $OutputPath)) {
        throw "Unable to find -OutputPath: $OutputPath"
    }

    if(!(Test-Path -Path $helpFolder)) {
        throw "Unable to find help folder: $helpFolder"
    }

    Write-Output "Generating external help: $OutputPath"
    [void](New-ExternalHelp -Path $helpFolder -OutputPath $OutputPath\$Language -Force `
                         -ErrorAction Stop)

    Write-Output "Finished generating external help"
}

if($TargetFramework) {
    $matchRegex = "(?<Front>RootModule = ')(?<DllName>.+)(?<Back>')"
    $addTargetFramework = ".\lib\$TargetFramework\"
    $replaceMatch = '${Front}' + $addTargetFramework + '${DllName}${Back}'

    $psdFileContent = Get-Content -Path $ModulePath -Raw
    if(($psdFileContent -match $matchRegex) -and (!($Matches['DllName'].StartsWith($addTargetFramework)))) {
        Write-Output "Adding '$addTargetFramework' to RootModule: $ModulePath"
        $psdFileContent = $psdFileContent -replace $matchRegex, $replaceMatch

        $utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($false)
        [System.IO.File]::WriteAllText($ModulePath, $psdFileContent, $utf8NoBomEncoding)
    }
    else {
        Write-Output "Module '$ModulePath' already contains target framework"
    }
}
