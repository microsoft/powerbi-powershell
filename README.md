# Microsoft Power BI Cmdlets for Windows PowerShell and PowerShell Core

Welcome to the PowerShell community for Microsoft Power BI. Here you will find resources and source for PowerShell modules targeting PowerBI.

For questions or issues using the modules, please log an issue and we will respond as quickly as possible.

## PowerShell Modules

Below is a table of the various Power BI PowerShell modules found in this repository.

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

>Two scopes are supported by cmdlets that interact with Power BI entities:
> * Individual is used to access entities that belong to the current user.
> * Organization is used to access entities across the entire company. Only Power BI tenant admins are allowed to use.

### Login to Power BI

```powershell
Connect-PowerBIServiceAccount   # or Login-PowerBIServiceAccount
```

### Get Workspaces

Get all workspaces for the user:

```powershell
Get-PowerBIWorkspace
```

### Update Workspace

Update the name or description of a user's workspace:

```powershell
Set-PowerBIWorkspace -Scope Organization -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -Name "Test Name" -Description "Test Description"
```

### Restore Workspace

Restores a deleted workspace:

```powershell
Restore-PowerBIWorkspace -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -Name "TestWorkspace" -EmailAddress "john@contoso.com"
```

### Add new user to workspace

Add a user to a given workspace:

```powershell
Add-PowerBIWorkspaceUser -Scope Organization -Id 3244f1c1-01cf-457f-9383-6035e4950fdc -UserEmailAddress john@contoso.com -UserAccessRight Admin
```

### Remove a user from a given workspace

Remove user's permissions from a given workspace:

```powershell
Remove-PowerBIWorkspaceUser -Scope Organization -Id 3244f1c1-01cf-457f-9383-6035e4950fdc -UserEmailAddress john@contoso.com
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
