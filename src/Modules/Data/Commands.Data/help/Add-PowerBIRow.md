---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Add-PowerBIRow

## SYNOPSIS
Adds rows to the specified table in a Power BI dataset.

## SYNTAX

### Dataset (Default)
```
Add-PowerBIRow -Dataset <Dataset> -TableName <String>
 -Rows <System.Collections.Generic.List`1[System.Management.Automation.PSObject]> [-WorkspaceId <Guid>]
 [-Workspace <Workspace>] [<CommonParameters>]
```

### DatasetId
```
Add-PowerBIRow -DatasetId <Guid> -TableName <String>
 -Rows <System.Collections.Generic.List`1[System.Management.Automation.PSObject]> [-WorkspaceId <Guid>]
 [-Workspace <Workspace>] [<CommonParameters>]
```

## DESCRIPTION
Inserts rows into a Power BI table contained within a dataset.

## EXAMPLES

### Example 1
```powershell
PS C:\>Add-PowerBIRow -DataSetId 4b644350-f745-48dd-821c-f008350199a8 -TableName Table1 -Rows @{"Column1"="Value1";"Column2"="Value2"},@{"Column1"="Value1";"Column2"="Value2"}
```

This example inserts two rows to Table1.

### Example 2
```powershell
PS C:\>Add-PowerBIRow -DataSetId 4b644350-f745-48dd-821c-f008350199a8 -TableName Table1 -Rows (Import-Csv -Path ".\data.csv")
```

This example inserts rows from CSV to Table1.

## PARAMETERS

### -Dataset
A dataset containing the table where rows are to be stored.

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
An ID of the dataset containing the table where rows are to be stored.

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
Workspace containing the dataset and table for row insertion.

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
Id of the workspace containing the dataset and table for row insertion.

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
