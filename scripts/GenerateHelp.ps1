##############################
#.SYNOPSIS
# Generates either Markdown or MAML (PowerShell external) help files for a PowerShell module.
#
#.DESCRIPTION
# Using PlatyPS, generates or updates Markdown files based on module's cmdlet structure or generates PowerShell exernal help files (MAML) based on the Markdown files.
#
#.EXAMPLE
# PS:> .\GenerateHelp.ps1 -GenerateMarkdown -ModulePath ..\src\Modules\Profile\Commands.Profile\bin\Debug\netstandard2.0\MicrosoftPowerBIMgmt.Profile.psd1 -ProjectRoot ..\src\Modules\Profile\Commands.Profile 
# Creates or updates Markdown help files in ..\src\Modules\Profile\Commands.Profile\help based on module ..\src\Modules\Profile\Commands.Profile\bin\Debug\netstandard2.0\MicrosoftPowerBIMgmt.Profile.psd1
#
#.EXAMPLE
# PS:> .\GenerateHelp.ps1 -GenerateExternalHelp -ProjectRoot ..\src\Modules\Profile\Commands.Profile -OutputPath ..\src\Modules\Profile\Commands.Profile\bin\Debug\netstandard2.0
# Generates MAML file under ..\src\Modules\Profile\Commands.Profile\bin\Debug\netstandard2.0
#
#.NOTES
# For more information on PlatyPS, see https://github.com/PowerShell/platyPS.
# For more information on MAML files, see https://msdn.microsoft.com/en-us/library/bb525433(v=vs.85).aspx.
##############################
[CmdletBinding()]
param
(
    # Path to module to generate markdown files to.
    [Parameter(Mandatory, ParameterSetName="GenerateMarkdown")]
    [string] $ModulePath,

    # Path to PlatyPS. Required if PlatyPS is not installed in $env:PSModulePath.
    [string] $PathToPlatyPS,

    # CSProj root directory.
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string] $ProjectRoot,

    # Output path to generate external help to.
    [Parameter(Mandatory, ParameterSetName="GenerateExternalHelp")]
    [string] $OutputPath,

    # Name of language to generate for. Default is en-US.
    [ValidateNotNullOrEmpty()]
    [string] $Language = 'en-US',

    # Indicates to generate Markdown files.
    [Parameter(Mandatory, ParameterSetName="GenerateMarkdown")]
    [switch] $GenerateMarkdown,

    # Indicates to generate external help (MAML) file.
    [Parameter(Mandatory, ParameterSetName="GenerateExternalHelp")]
    [switch] $GenerateExternalHelp,

    # List of dependent assemblies to load if loading module fails to resolve. Semi-colon seperated.
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