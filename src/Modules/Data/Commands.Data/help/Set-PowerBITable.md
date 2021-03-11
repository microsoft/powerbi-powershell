---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Set-PowerBITable

## SYNOPSIS
Updates the metadata and schema for the specified Power BI table.

## SYNTAX

### DatasetId (Default)
```
Set-PowerBITable -Table <Table> -DatasetId <Guid> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [<CommonParameters>]
```

### Dataset
```
Set-PowerBITable -Table <Table> -Dataset <Dataset> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [<CommonParameters>]
```

## DESCRIPTION
Updates the metadata and schema for the specified Power BI table.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount

## EXAMPLES

### Example 1
```powershell
$currentTables = Get-PowerBITable -DatasetId c47f6cff-70de-4837-a094-93a6f26e20bf

$currentTable = $currentTables[0]
$col1 = New-PowerBIColumn -Name Col1 -DataType Int64
$col2 = New-PowerBIColumn -Name Col2 -DataType String
$updatedTable = New-PowerBITable -Name $currentTable.Name -Columns $col1,$col2

Set-PowerBITable -Table $updatedTable -DatasetId c47f6cff-70de-4837-a094-93a6f26e20bf
```

This example retrieves current table and create new one from the table.
Then, it updates the table schema.

## PARAMETERS

### -Dataset
A dataset where tables are stored.

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
ID of the dataset where tables are to be stored.

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

### -Table
Table to update the schema.

```yaml
Type: Table
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
ID of the workspace to filter the place where table resides.

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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Dataset
Microsoft.PowerBI.Common.Api.Workspaces.Workspace

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## NOTES

## RELATED LINKS
