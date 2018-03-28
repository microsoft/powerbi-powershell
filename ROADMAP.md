# Microsoft Power BI Cmdlets Roadmap

Planned\upcoming cmdlets.

> Cmdlets follow the following format: `%Verb%-PowerBI%Noun%`  
> _%Verb%_ must be an approved verb, use `Get-Verb` in PowerShell or visit [here][1] to see list.  
>
> Names should also conform to these two guidelines: 
> * [Required Development Guidelines][3]
> * [Strongly Encouraged Development Guidelines][2]

## General Improvments
* Adding -Scope parameter to more cmdlets
* Supporting PowerShell Core (dependent on updating PowerBI.Api and Microsoft.Rest.ClientRuntime to .NET Standard 2.0)

## Workspaces

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| New-PowerBIWorkspace | Creates a new workspace | Create-PowerBIWorkspace |
| Remove-PowerBIWorkspace | Deletes a workspace | | 
| Set-PowerBICapacityOnWorkspace | Allocates capcity to a Workspace | Assign-PowerBICapacityToWorkspace |

## Reports

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Copy-PowerBIReport | Clone a report | 
| Remove-PowerBIReport | Delete a report |
| Export-PowerBIReport  | Export a report
| New-PowerBIReportEmbedToken | Generate an embed token |
| Add-PowerBIDatasetToReport | Rebind dataset to report | Mount-PowerBIDatasetToReport
| Update-PowerBIReport | Update a report |

## Dashboards

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Get-PowerBIDashbord | Get dashboards |
| Add-PowerBIDashbord | Add dashboards |
| Copy-PowerBITile | Clone tiles |
| New-PowerBIDashboardEmbedToken | Generate an embed token |
| Get-PowerBITile | Get tiles |

## Datasets and Datasources

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Get-PowerBIDataset | Get datasets |
| Get-PowerBIDatasource | Get datasources |
| Get-PowerBIDatasetParameter | Get parameters for dataset |
| Add-PowerBIGatewayToDatasource | Bind to a gateway | Mount-PowerBIGatewayToDatasource
| Remove-PowerBIDataSource | Delete dataset |
| Remove-PowerBIDatasetRow | Delete dataset rows |
| Get-PowerBIGatewayForDataset | Discover gateways | 
| New-PowerBIDatasetEmbedToken | Generate an embed token |
| Get-PowerBIDatasetRefreshHistory | Get refresh history |
| Get-PowerBITablesInDatasource | Get tables in datasource |
| New-PowerBIDatasetData | Create data in dataset |
| New-PowerBIDatasetRow | Create rows in datasource tables |
| Update-PowerBIDataset | Refresh dataset | Refresh-PowerBIDataset
| Update-PowerBIDataSourceSchema | Update schema\metadata in datasource table |
| Protect-PowerBIDataset | Take over a dataset | TakeOver-PowerBIDataset
| Update-PowerBIDatasource | Update a datasource |
| Update-PowerBIDatasetParameter | Update parameters for a dataset |

## Capacity Management

| Cmdlet Name | Purpose | Alias | Notes |
| ----------- | ------- | ----- | ----- |
| Get-PowerBICapacity | Get capacities |

## Tenant Settings

To be defined

## Imports

To be defined.

## Gateway

If applicable, should reside in https://www.powershellgallery.com/packages/OnPremisesDataGatewayMgmt


[1]: https://msdn.microsoft.com/en-us/library/ms714428(v=vs.85).aspx
[2]: https://msdn.microsoft.com/en-us/library/dd878270(v=vs.85).aspx
[3]: https://msdn.microsoft.com/en-us/library/dd878238(v=vs.85).aspx