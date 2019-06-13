# Microsoft Power BI Cmdlets for Windows PowerShell and PowerShell Core

Welcome to the PowerShell community for Microsoft Power BI. Here you will find resources and source for PowerShell modules targeting PowerBI.

For questions or issues using the modules, please log an issue and we will respond as quickly as possible.

## PowerShell Modules

Below is a table of the various Power BI PowerShell modules found in this repository.

| Description | Module Name | PowerShell Gallery link |
| ----------- | ----------- | ----------------------- |
| Rollup Module for Power BI Cmdlets | `MicrosoftPowerBIMgmt` | [![MicrosoftPowerBIMgmt](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.svg?style=flat-square&label=MicrosoftPowerBIMgmt)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt/) |
| Admin module for Power BI Cmdlets | [MicrosoftPowerBIMgmt.Admin](https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.admin) | [![MicrosoftPowerBIMgmt.Admin](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.Admin.svg?style=flat-square&label=MicrosoftPowerBIMgmt.Admin)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt.Admin/) |
| Capacities module for Power BI Cmdlets | [MicrosoftPowerBIMgmt.Capacities](https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.capacities) | [![MicrosoftPowerBIMgmt.Capacities](https://img.shields.io/powershellgallery/v/MicrosoftPowerBIMgmt.Capacities.svg?style=flat-square&label=MicrosoftPowerBIMgmt.Capacities)](https://www.powershellgallery.com/packages/MicrosoftPowerBIMgmt.Capacities/) |
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

If the `-Scope` parameter doesn't exist on the cmdlet, the entity doesn't support an Administrative API.

### Log in to Power BI

```powershell
Connect-PowerBIServiceAccount   # or use aliases: Login-PowerBIServiceAccount, Login-PowerBI
```

### Get Workspaces

Get workspaces for the user. By default (i.e. without `-First` parameter) it shows the first 100 workspaces assigned to the user:

```powershell
Get-PowerBIWorkspace
```

Use the `-All` parameter to show all workspaces assigned to the user:

```powershell
Get-PowerBIWorkspace -All
```

If you are a tenant administrator, you can view all workspaces in your tenant by adding `-Scope Organization`:

```powershell
Get-PowerBIWorkspace -Scope Organization -All
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

To view deleted workspaces as a tenant administrator:

```powershell
Get-PowerBIWorkspace -Scope Organization -Deleted -All
```

Restores a deleted workspace:

```powershell
Restore-PowerBIWorkspace -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -RestoredName "TestWorkspace" -AdminEmailAddress "john@contoso.com"
```

### Recover orphaned workspaces

A workspace becomes orphaned when it has no assigned administrators. If you are a tenant administrator, run the following to view all orphaned workspaces:

```powershell
Get-PowerBIWorkspace -Scope Organization -Orphaned -All
```

To correct this issue, use:

```powershell
Add-PowerBIWorkspaceUser -Scope Organization -Id f2a0fae5-1c37-4ee6-97da-c9d31851fe17 -UserPrincipalName 'john@contoso.com' -AccessRight Admin
```

### Get Reports

Get all reports for the user:

```powershell
Get-PowerBIReport
```

If you are a tenant administrator, you can view all reports in your tenant by using assigning `-Scope Organization`:

```powershell
Get-PowerBIReport -Scope Organization
```

### Get Dashboards

Get dashboards for the user:

```powershell
Get-PowerBIDashboard
```

If you are a tenant administrator, you can view all dashboards in your tenant by adding `-Scope Organization`:

```powershell
Get-PowerBIDashboard -Scope Organization
```

### Get Tiles

Get tiles within a dashboard:

```powershell
Get-PowerBITile -DashboardId 9a58d5e5-61bc-447c-86c4-e221128b1c99
```

### Get Imports

Get Power BI imports:

```powershell
Get-PowerBIImport
```

### Create Report

Create a report in Power BI by uploading a \*.pbix file:

```powershell
New-PowerBIReport -Path .\newReport.pbix -Name 'New Report'
```

By default, the report is placed in the user's My Workspace. To place in a different workspace, use the `-WorkspaceId` or `-Workspace` parameters:

```powershell
New-PowerBIReport -Path .\newReport.pbix -Name 'New Report' -WorkspaceId f95755a1-950c-46bd-a912-5aab4012a06d
```

### Export Report

Export a Power BI report to \*.pbix file:

```powershell
Export-PowerBIReport -Id b48c088c-6f4e-4b7a-b015-d844ab534b2a -OutFile .\exportedReport.pbix
```

If the workspace exists outside the My Workspace, export with the `WorkspaceId` or `-Workspace` parameter:

```powershell
Export-PowerBIReport -Id b48c088c-6f4e-4b7a-b015-d844ab534b2a -OutFile .\exportedReport.pbix -WorkspaceId 3bdd9735-0ab5-4f21-bd5d-87e7f1d7fb84
```

### Get Datasets

Get Power BI datasets:

```powershell
Get-PowerBIDataset
```

### Get Datasources

Get Power BI datasources for a dataset:

```powershell
Get-PowerBIDatasource -DatasetId 65d7d7e5-8af0-4e94-b20b-50a882ae15e1
```

### Get Tables

Get Power BI tables contained within a dataset:

```powershell
Get-PowerBITable -DatasetId 65d7d7e5-8af0-4e94-b20b-50a882ae15e1
```

### Call the Power BI Rest API

For [Power BI API](https://docs.microsoft.com/en-us/rest/api/power-bi/) that lacks corresponding cmdlets, you can reuse the authenticated session from `Connect-PowerBIServiceAccount` to make custom REST requests:

```powershell
Invoke-PowerBIRestMethod -Url 'reports/4eb4c303-d5ac-4a2d-bf1e-39b35075d983/Clone' -Method Post -Body ([pscustomobject]@{name='Cloned report'; targetModelId='adf823b5-a0de-4b9f-bcce-b17d774d2961'; targetWorkspaceId='45ee15a7-0e8e-45b0-8111-ea304ada8d7d'} | ConvertTo-Json -Depth 2 -Compress)
```

If you want to use the authenticated session outside of PowerShell, get the access token by using:

```powershell
Get-PowerBIAccessToken -AsString
```

### Troubleshooting Errors

To get more information about an error returned back from the cmdlets, use:

```powershell
Resolve-PowerBIError -Last
```

This information can be useful for opening support tickets for Power BI.

## Issues and Feedback

If you find any bugs or would like to see certain functionality implemented for the PowerShell Cmdlets for Power BI, please file an issue [here](https://github.com/Microsoft/powerbi-powershell/issues). If the issue is an error returned from the cmdlets, add detail from `Resolve-PowerBIError` to the issue.

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
