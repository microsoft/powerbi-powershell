---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Get-PowerBIDatasource

## SYNOPSIS
Returns a list of Power BI datasources.

## SYNTAX

### List (Default)
```
Get-PowerBIDatasource -DatasetId <Guid> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Id
```
Get-PowerBIDatasource -DatasetId <Guid> [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### Name
```
Get-PowerBIDatasource -DatasetId <Guid> [-WorkspaceId <Guid>] -Name <String> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### ObjectAndId
```
Get-PowerBIDatasource -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### ObjectAndName
```
Get-PowerBIDatasource -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### ObjectAndList
```
Get-PowerBIDatasource -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI datasources under the specified dataset along that match the specified search criteria and scope.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDatasource -DatasetId 23d088a0-a395-483e-b81c-54f51f3e4e3c
```

Returns all datasources in Power BI dataset with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c, for dataset the calling user has access to.

### Example 2
```powershell
PS C:\> Get-PowerBIDatasource -DatasetId 23d088a0-a395-483e-b81c-54f51f3e4e3c -Scope Organization
```

Returns all datasources in Power BI dataset with ID 23d088a0-a395-483e-b81c-54f51f3e4e3c, for a dataset in the calling user's organization.

## PARAMETERS

### -Dataset
Dataset for returning datasources for.

```yaml
Type: Dataset
Parameter Sets: ObjectAndId, ObjectAndName, ObjectAndList
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DatasetId
Id of the dataset to return datasources for.

```yaml
Type: Guid
Parameter Sets: List, Id, Name
Aliases: DatasetKey

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Id of the datasource to return.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: DatasourceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the datasource to return if one exists with that name. Case insensitive search.

```yaml
Type: String
Parameter Sets: Name
Aliases: DatasoureName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual returns only datasources assigned to the caller; Organization returns all datasources within a tenant (must be an administrator to initiate). Individual is the default.

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
Workspace ID to filter results to, datasources only belonging to that workspace are shown. Only available when -Scope is Individual.

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

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
