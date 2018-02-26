# Microsoft PowerBI Cmdlets for Windows PowerShell and PowerShell Core

Welcome to the PowerShell community for Microsoft PowerBI. Here you will find resources and source for PowerShell modules targeting PowerBI.

For questions or issues using the modules, please log an issue and we will respond as quickly as possible.

## PowerShell Modules

Below is a table of the various PowerBI PowerShell modules found in this repository.

| Description | Module Name | PowerShell Gallery link |
| ----------- | ----------- | ----------------------- |
| Rollup Module for PowerBI Cmdlets | `MicrosoftPowerBIMgmt` | TBD |
| Profile modules for PowerBI Cmdlets | `MicrosoftPowerBIMgmt.Profile` | TBD |
| Workspace module for PowerBI | `MicrosoftPowerBIMgmt.Workspace` | TBD |
| Reports module for PowerBI | `MicrosoftPowerBIMgmt.Reports` | TBD |

## Supported Environments and PowerShell Versions

* Windows PowerShell v3.0 and up with .NET 4.6.1 or above.
* PowerShell Core (v6) and up on any OS platform supported by PowerShell Core.

## Installation

Currently you must build the cmdlets and load them manually but eventually they will be available on PowerShell Gallery.

### PowerShell Gallery

We haven't published to PowerShell Gallery which is TBD, but will once we build out more cmdlets.

## Usage

### Login to PowerBI

```powershell
Connect-PowerBIServiceAccount   # or Login-PowerBIServiceAccount
```

### Getting Workspaces

Fetching all user's workspaces:

```powershell
Get-PowerBIWorkspace
```

### Getting Reports

Fetching all user's reports:

```powershell
Get-PowerBIReport
```

## Issues and Feedback

If you find any bugs or would like to see certain functionality implemented for the PowerShell Cmdlets for PowerBI, please file an issue [here](https://github.com/Microsoft/powerbi-powershell/issues).

If your issue is broader than just the PowerShell cmdlets, please submit your feedback to the [PowerBI Community](http://community.powerbi.com/) or the official [PowerBI Support](https://powerbi.microsoft.com/en-us/support/) site.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

More information about contributing can be found in this [CONTRIBUTING](CONTRIBUTING.md) guide.
