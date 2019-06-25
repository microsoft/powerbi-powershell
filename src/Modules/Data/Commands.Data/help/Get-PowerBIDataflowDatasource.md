---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.data/get-powerbidataflowdatasource?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIDataflowDatasource

## SYNOPSIS
Returns a list of Power BI data sources for the given Dataflow.

## SYNTAX

### List (Default)
```
Get-PowerBIDataflowDatasource -DataflowId <String> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### Id
```
Get-PowerBIDataflowDatasource -DataflowId <String> [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### Name
```
Get-PowerBIDataflowDatasource -DataflowId <String> [-WorkspaceId <Guid>] -Name <String> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### ObjectAndId
```
Get-PowerBIDataflowDatasource -Dataflow <Dataflow> [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### ObjectAndName
```
Get-PowerBIDataflowDatasource -Dataflow <Dataflow> [-WorkspaceId <Guid>] -Name <String> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### ObjectAndList
```
Get-PowerBIDataflowDatasource -Dataflow <Dataflow> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI data sources under the specified dataflow along that match the specified search criteria and scope.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDatasource -DataflowId 23d088a0-a395-483e-b81c-54f51f3e4e3c -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375
```

Returns all data sources in Power BI dataflow with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c, for dataflow the calling user has access to.

### Example 2
```powershell
PS C:\> Get-PowerBIDatasource -DataflowId 23d088a0-a395-483e-b81c-54f51f3e4e3c -Scope Organization
```

Returns all data sources in Power BI dataflow with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c, for a dataflow in the calling user's organization.

## PARAMETERS

### -Dataflow
Dataflow for returning data sources for.

```yaml
Type: Dataflow
Parameter Sets: ObjectAndId, ObjectAndName, ObjectAndList
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DataflowId
ID of the dataflow to return data sources for.

```yaml
Type: Guid
Parameter Sets: List, Id, Name
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the data source to return.

```yaml
Type: Guid
Parameter Sets: Id, ObjectAndId
Aliases: DatasourceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the data source to return if one exists with that name. Case insensitive search.

```yaml
Type: String
Parameter Sets: Name, ObjectAndName
Aliases: DatasoureName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual returns only data sources assigned to the caller; Organization returns all data sources within a tenant (must be an administrator to initiate). Individual is the default.

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
ID of the workspace to filter results to, data sources only belonging to that workspace are shown. Only available when -Scope is Individual.

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
