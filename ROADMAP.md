# Microsoft Power BI Cmdlets Roadmap

Planned\upcoming cmdlets.

> Cmdlets follow the following format: `%Verb%-PowerBI%Noun%`  
> _%Verb%_ must be an approved verb, use `Get-Verb` in PowerShell or visit [here][1] to see list.  
>
> Names should also conform to these two guidelines:
> * [Required Development Guidelines][3]
> * [Strongly Encouraged Development Guidelines][2]

## General Improvements

* Adding -Scope parameter to more cmdlets (dependent on building new APIs)

## Workspaces

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| New-PowerBIWorkspace | Creates a new workspace | Create-PowerBIWorkspace |
| Remove-PowerBIWorkspace | Deletes a workspace | Delete-PowerBIWorkspace | 
| Set-PowerBICapacityOnWorkspace | Allocates capacity to a Workspace | Assign-PowerBICapacityToWorkspace |

## Reports

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Copy-PowerBIReport | Clone a report | 
| Remove-PowerBIReport | Delete a report | Delete-PowerBIReport |
| Export-PowerBIReport  | Export a report | | Add support for -Scope Organization |
| New-PowerBIReportEmbedToken | Generate an embed token |
| Add-PowerBIDatasetToReport | Rebind dataset to report | Mount-PowerBIDatasetToReport |
| Update-PowerBIReport | Update a report |

## Dashboards

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Add-PowerBIDashboard | Add dashboards |
| Copy-PowerBITile | Clone tiles |
| New-PowerBIDashboardEmbedToken | Generate an embed token |

## Datasets and Datasources

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Get-PowerBIDatasetParameter | Get parameters for dataset |
| Add-PowerBIGatewayToDatasource | Bind to a gateway | Mount-PowerBIGatewayToDatasource
| Remove-PowerBIDatasource | Delete dataset | Delete-PowerBIDatasource |
| Remove-PowerBIDatasetRow | Delete dataset rows | Delete-PowerBIDatasetRow |
| Get-PowerBIGatewayForDataset | Discover gateways |
| New-PowerBIDatasetEmbedToken | Generate an embed token |
| Get-PowerBIDatasetRefreshHistory | Get refresh history |
| New-PowerBIDatasetRow | Create rows in data source tables |
| Update-PowerBIDataset | Refresh dataset | Refresh-PowerBIDataset
| Update-PowerBITableSchema | Update schema\metadata in data source table |
| Protect-PowerBIDataset | Take over a dataset | TakeOver-PowerBIDataset
| Set-PowerBIDatasource | Update a data source | Update-PowerBIDatasource |
| Set-PowerBIDatasetParameter | Update parameters for a dataset | Update-PowerBIDatasetParameter |

## Capacity Management

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Get-PowerBICapacity | Get capacities |

## Tenant Settings

To be defined


[1]: https://msdn.microsoft.com/en-us/library/ms714428(v=vs.85).aspx
[2]: https://msdn.microsoft.com/en-us/library/dd878270(v=vs.85).aspx
[3]: https://msdn.microsoft.com/en-us/library/dd878238(v=vs.85).aspx