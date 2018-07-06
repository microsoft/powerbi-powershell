---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Set-PowerBITable

## SYNOPSIS
Updates the metadata and schema for the specified table.

## SYNTAX

### DatasetId (Default)
```
Set-PowerBITable -Table <Table> -DatasetId <Guid> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Dataset
```
Set-PowerBITable -Table <Table> -Dataset <Dataset> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
Set-PowerBITable lets you update the metadata and schema for the specified table
Before you run this command, make sure you log in using Login-PowerBIServiceAccount. 

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
A dataset where tables stored. You can pass it via pipe.

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
An id of dataset where tables stored.

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

### -Scope
Only Individual scope is supported.

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

### -Table
Table to be updated.

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
WorkspaceId to filter the place where table resides.

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
