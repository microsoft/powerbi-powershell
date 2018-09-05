---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Remove-PowerBIRow

## SYNOPSIS
Removes rows from a Power BI table.

## SYNTAX

### Dataset (Default)
```
Remove-PowerBIRow -Dataset <Dataset> -TableName <String> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [<CommonParameters>]
```

### DatasetId
```
Remove-PowerBIRow -DatasetId <Guid> -TableName <String> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [<CommonParameters>]
```

## DESCRIPTION
Performs removal of rows from a Power BI table.

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-PowerBIRow -DataSetId 4b644350-f745-48dd-821c-f008350199a8 -TableName Table1
```

Removes rows from the table Table1 in specified dataset.

## PARAMETERS

### -Dataset
A dataset containing the table where rows are to be removed.

```yaml
Type: Dataset
Parameter Sets: Dataset
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DatasetId
ID of the dataset containing the table where rows are to be removed.

```yaml
Type: Guid
Parameter Sets: DatasetId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TableName
Name of the table.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace containing the dataset and table for row removal.

```yaml
Type: Workspace
Parameter Sets: (All)
Aliases: Group

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WorkspaceId
ID of the workspace containing the dataset and table for row removal.

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
Microsoft.PowerBI.Common.Api.Workspaces.Workspace

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## NOTES

## RELATED LINKS
