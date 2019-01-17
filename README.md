# Microsoft Power BI Cmdlets for Windows PowerShell and PowerShell Core

Welcome to the PowerShell community for Microsoft Power BI. Here you will find resources and source for PowerShell modules targeting PowerBI.

For questions or issues using the modules, please log an issue and we will respond as quickly as possible.

## PowerShell Modules

Below is a table of the various Power BI PowerShell modules found in this repository.

| Description | Module Name | PowerShell Gallery link |
| ----------- | ----------- | ----------------------- |
| Rollup Module for Power BI Cmdlets | `MicrosoftPowerBIMgmt` | [![MicrosoftPowerBIMgmt](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.svg?style=flat-square&label=MicrosoftPowerBIMgmt)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt/) |
| Data module for Power BI Cmdlets | [MicrosoftPowerBIMgmt.Data](https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.data) | [![MicrosoftPowerBIMgmt.Data](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.Data.svg?style=flat-square&label=MicrosoftPowerBIMgmt.Data)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt.Data/) |
| Profile module for Power BI Cmdlets | [MicrosoftPowerBIMgmt.Profile](https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.profile) | [![MicrosoftPowerBIMgmt.Profile](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.Profile.svg?style=flat-square&label=MicrosoftPowerBIMgmt.Profile)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt.Profile/) |
| Reports module for Power BI | [MicrosoftPowerBIMgmt.Reports](https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.reports) | [![MicrosoftPowerBIMgmt.Reports](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.Reports.svg?style=flat-square&label=MicrosoftPowerBIMgmt.Reports)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt.Reports/) |
| Workspaces module for Power BI | [MicrosoftPowerBIMgmt.Workspaces](https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.workspaces) | [![MicrosoftPowerBIMgmt.Workspaces](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.Workspaces.svg?style=flat-square&label=MicrosoftPowerBIMgmt.Workspaces)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt.Workspaces/) |

More documentation can be found at https://docs.microsoft.com/en-us/powershell/power-bi/overview.

## Supported Environments and PowerShell Versions

* Windows PowerShell v3.0 and up with .NET 4.7.1 or above.
* PowerShell Core (v6) and up on any OS platform supported by PowerShell Core.

## Installation

The cmdlets are available on PowerShell Gallery and can be installed in an elevated PowerShell session:

```powershell
Install-Module -Name MicrosoftPowerBIMgmt
```

Optionally you could install individual modules (based on your needs) instead of the rollup module, for example if you only wanted the Workspaces module:

```powershell
Install-Module -Name MicrosoftPowerBIMgmt.Workspaces
```

If you have an earlier version, you can update to the latest version by running:

```powershell
Update-Module -Name MicrosoftPowerBIMgmt
```

### Uninstall

If you want to uninstall all the Power BI PowerShell cmdlets, run the following in an elevated PowerShell session:

```powershell
Get-Module MicrosoftPowerBIMgmt* -ListAvailable | Uninstall-Module -Force
```

## Usage

>Two scopes are supported by cmdlets that interact with Power BI entities:
> * Individual is used to access entities that belong to the current user.
> * Organization is used to access entities across the entire company. Only Power BI tenant admins are allowed to use.

### Log in to Power BI

```powershell
Connect-PowerBIServiceAccount   # or Login-PowerBIServiceAccount
```

### Get Workspaces

Get workspaces for the user. By default (without [-First](https://github.com/Microsoft/powerbi-powershell/blob/master/src/Modules/Workspaces/Commands.Workspaces/help/Get-PowerBIWorkspace.md#-first)) it returns the first 100 workspaces.:

```powershell
Get-PowerBIWorkspace
```

Introduced [-All](https://github.com/Microsoft/powerbi-powershell/blob/master/src/Modules/Workspaces/Commands.Workspaces/help/Get-PowerBIWorkspace.md#-all) parameter to retrieve all the workspaces for the user.

```powershell
Get-PowerBIWorkspace -All
```

### Update Workspace

Update the name or description of a user's workspace:

```powershell
Set-PowerBIWorkspace -Scope Organization -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -Name "Test Name" -Description "Test Description"
```

### Add new user to workspace

Add a user to a given workspace:

```powershell
Add-PowerBIWorkspaceUser -Scope Organization -Id 3244f1c1-01cf-457f-9383-6035e4950fdc -UserEmailAddress john@contoso.com -AccessRight Admin
```

### Remove a user from a given workspace

Remove user's permissions from a given workspace:

```powershell
Remove-PowerBIWorkspaceUser -Scope Organization -Id 3244f1c1-01cf-457f-9383-6035e4950fdc -UserEmailAddress john@contoso.com
```

### Restore Workspace

Restores a deleted workspace:

```powershell
Restore-PowerBIWorkspace -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -RestoredName "TestWorkspace" -AdminEmailAddress "john@contoso.com"
```

### Get Reports

Get all reports for the user:

```powershell
Get-PowerBIReport
```

## Issues and Feedback

If you find any bugs or would like to see certain functionality implemented for the PowerShell Cmdlets for Power BI, please file an issue [here](https://github.com/Microsoft/powerbi-powershell/issues).

If your issue is broader than just the PowerShell cmdlets, please submit your feedback to the [Power BI Community](http://community.powerbi.com/) or the official [Power BI Support](https://powerbi.microsoft.com/en-us/support/) site.

We track our roadmap of planned features in [ROADMAP.md](ROADMAP.md).

### Reporting Security Issues

Security issues and bugs should be reported privately, via email, to the Microsoft Security Response Center (MSRC) at [secure@microsoft.com](mailto:secure@microsoft.com).

You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message.
Further information, including the [MSRC PGP](https://technet.microsoft.com/en-us/security/dn606155) key, can be found in the [Security TechCenter](https://technet.microsoft.com/en-us/security/default).

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
