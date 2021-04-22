# Microsoft Power BI dataflows samples  

The document below describes the various powershell scripts available for Power BI dataflows. These rely on the Power BI public REST APIs and the Power BI PowerShell modules. 


## Power BI Dataflow PowerShell scripts

Below is a table of the various Power BI PowerShell modules found in this repository. 

| Description | Module Name |
| ----------- | ----------- |
| Export all dataflows from a workspace  | [ExportWorkspace.ps1](https://github.com/microsoft/powerbi-powershell/tree/master/examples/dataflows) 
| Imports all dataflows into a workspace  | [ImportWorkspace.ps1](https://github.com/microsoft/powerbi-powershell/tree/master/examples/dataflows) 
| Imports a single dataflow  | [ImportModel.ps1](https://github.com/microsoft/powerbi-powershell/tree/master/examples/dataflows) 
| Refresh a single dataflow | [RefreshModel.ps1](https://github.com/microsoft/powerbi-powershell/tree/master/examples/dataflows) 

For more information on Powershell support for Power BI, please visit [powerbi-powershell](https://docs.microsoft.com/en-us/powershell/power-bi/overview) on GitHub 



## Supported environments and PowerShell versions

* Windows PowerShell v3.0 and up with .NET 4.7.1 or above.
* PowerShell Core (v6) and up on any OS platform supported by PowerShell Core.


## Installation

1. The scripts depend on the MicrosoftPowerBIMgmt module which can be installed as follows:
        
    ```powershell
    Install-Module -Name MicrosoftPowerBIMgmt
    ```

2. If you have an earlier version, you can update to the latest version by running: 
        
    ```powershell
    Update-Module -Name MicrosoftPowerBIMgmt
    ```
3. Download all the scripts from the [GitHub Location](https://github.com/microsoft/powerbi-powershell/tree/master/examples/dataflows) into a local folder. 

4. Unblock the script by right click on the files and select “Unblock” after you download. Otherwise you might get a warning when you run the script. 



## Uninstall

If you want to uninstall all the Power BI PowerShell cmdlets, run the following in an elevated PowerShell session: 

```powershell
Get-Module MicrosoftPowerBIMgmt* -ListAvailable | Uninstall-Module -Force
```

## Usage

The APIs below supports two optional parameters:  

    -Environment: A flag to indicate specific Power BI environments to log in to (Public, Germany, USGov, China, USGovHigh, USGovMil). Default is Public 
    -V: A flag to indicate whether to produce verbose output. Default is false. 

### Export workspace

Exports all the dataflow model.json from a Power BI workspace into a folder: 

```powershell
.\ExportWorkspace.ps1 -Workspace "Workspace1" -Location C:\dataflows 
```

### Import workspace

Imports all the dataflow model.json from a folder into a Power BI workspace. This script also fixes the reference models to point to the right dataflow in the current workspace: 

```powershell
.\ImportWorkspace.ps1 -Workspace "Workspace1" -Location C:\dataflows -Overwrite
```

### Import dataflow

Imports a dataflow model.json into a Power BI workspace:

```powershell
.\ImportModel.ps1 -Workspace "Workspace1" -File C:\MyModel.json -Overwrite
```

### Refresh dataflow

Refreshes a dataflow and optionally triggers the refresh of depending datasets.

```powershell
.\RefreshModel.ps1 -Workspace "Workspace1" -Dataflow "Dataflow1" -RefreshDatasets
```