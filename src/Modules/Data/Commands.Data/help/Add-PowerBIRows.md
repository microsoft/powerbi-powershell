---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Add-PowerBIRows

## SYNOPSIS
Insert rows to the specified table in Power BI.

## SYNTAX

### Dataset (Default)
```
Add-PowerBIRows -Dataset <Dataset> -TableName <String>
 -Rows <System.Collections.Generic.List`1[System.Management.Automation.PSObject]> [-WorkspaceId <Guid>]
 [-Workspace <Workspace>] [<CommonParameters>]
```

### DatasetId
```
Add-PowerBIRows -DatasetId <Guid> -TableName <String>
 -Rows <System.Collections.Generic.List`1[System.Management.Automation.PSObject]> [-WorkspaceId <Guid>]
 [-Workspace <Workspace>] [<CommonParameters>]
```

## DESCRIPTION
Add-PowerBIRows creates new rows to the specified table.

## EXAMPLES

### Example 1
```powershell
PS C:\>Add-PowerBIRows -DataSetId 4b644350-f745-48dd-821c-f008350199a8 -TableName Table1 -Rows @{"Column1"="Value1";"Column2"="Value2"},@{"Column1"="Value1";"Column2"="Value2"}
```

This example inserts two rows to Table1.

### Example 2
```powershell
PS C:\>Add-PowerBIRows -DataSetId 4b644350-f745-48dd-821c-f008350199a8 -TableName Table1 -Rows (Import-Csv -Path ".\data.csv")
```

This example inserts rows from CSV to Table1.

## PARAMETERS

### -Dataset
A dataset of the table where rows are to be stored.

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
An id of dataset of the table where rows are to be stored.

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

### -Rows
An array of rows to be stored in the table.

```yaml
Type: System.Collections.Generic.List`1[System.Management.Automation.PSObject]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TableName
Name of the table which rows to be stored.

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
Workspace to filter the place where table resides.

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
The id of the workspace to filter the place where table resides.

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
