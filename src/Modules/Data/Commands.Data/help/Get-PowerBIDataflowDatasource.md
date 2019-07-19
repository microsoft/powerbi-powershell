---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.data/get-powerbidataflowdatasource?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIDataflowDatasource

## SYNOPSIS
Returns a list of Power BI datasources for the given Dataflow.

## SYNTAX

### List (Default)
```
Get-PowerBIDataflowDatasource -DataflowId <Guid> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### Id
```
Get-PowerBIDataflowDatasource -DataflowId <Guid> [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### DataflowAndId
```
Get-PowerBIDataflowDatasource -Dataflow <Dataflow> [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### DataflowAndList
```
Get-PowerBIDataflowDatasource -Dataflow <Dataflow> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI datasources under the specified dataflow that match the specified search criteria and scope.
Datasources connected to Dataflows doesn't have a Name value, so there is no Name parameter.
For -Scope Individual, user must specify the dataflow's Workspace, using the given -WorkspaceId value.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDatasource -DataflowId 23d088a0-a395-483e-b81c-54f51f3e4e3c -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375
```

Returns all datasources in a Power BI dataflow with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c, for a dataflow the calling user has access to.

### Example 2
```powershell
PS C:\> Get-PowerBIDatasource -DataflowId 23d088a0-a395-483e-b81c-54f51f3e4e3c -Scope Organization
```

Returns all datasources in a Power BI dataflow with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c, for a dataflow in the calling user's organization.

### Example 3
```powershell
PS C:\> Get-PowerBIDatasource -DataflowId 23d088a0-a395-483e-b81c-54f51f3e4e3c -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375 -Id a3066be2-ea12-4ae2-b8e9-b8006b1fbf61
```

Returns a datasource with ID a3066be2-ea12-4ae2-b8e9-b8006b1fbf61, in a dataflow with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c the calling user has access to.

### Example 4
```powershell
PS C:\> Get-PowerBIDataflow -Scope Organization -Name "MyDataflow" | Get-PowerBIDatasource -Scope Organization
```

Returns all datasources in a Power BI dataflow with Name "MyDataflow", using a pipeline result from a call to Get-PowerBIDataflow.

## PARAMETERS

### -Dataflow
Dataflow for returning datasources for.

```yaml
Type: Dataflow
Parameter Sets: DataflowAndId, DataflowAndList
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DataflowId
ID of the dataflow to return datasources for.

```yaml
Type: Guid
Parameter Sets: List, Id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the datasource to return.

```yaml
Type: Guid
Parameter Sets: Id, DataflowAndId
Aliases: DatasourceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual returns only datasources from dataflows assigned to the caller; Organization returns datasources for all dataflows within a tenant (must be an administrator to initiate). Individual is the default.

```yaml
Type: PowerBIUserScope
Parameter Sets: (All)
Aliases:
Accepted values: Individual, Organization

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
ID of the workspace to filter results to, datasources only belonging to that workspace are shown. Only available when -Scope is Individual.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: GroupId

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Microsoft.PowerBI.Common.Api.Dataflows.Dataflow

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
