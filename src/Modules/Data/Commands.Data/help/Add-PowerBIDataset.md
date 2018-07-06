---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Add-PowerBIDataset

## SYNOPSIS
Creates new dataset to PowerBI.

## SYNTAX

### Dataset (Default)
```
Add-PowerBIDataset -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Workspace
```
Add-PowerBIDataset -Dataset <Dataset> -Workspace <Workspace> [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
Add-PowerBIDataset creates new push dataset to PowerBI.

## EXAMPLES

### Example 1
```powershell
PS C:\>$col1 = New-PowerBIColumn -Name ID -DataType Int64
PS C:\>$col2 = New-PowerBIColumn -Name Data -DataType String
PS C:\>$table1 = New-PowerBITable -Name SampleTable1 -Columns $col1,$col2
PS C:\>
PS C:\>$col3 = New-PowerBIColumn -Name ID -DataType Int64
PS C:\>$col4 = New-PowerBIColumn -Name Date -DataType DateTime
PS C:\>$col5 = New-PowerBIColumn -Name Detail -DataType String
PS C:\>$col6 = New-PowerBIColumn -Name Result -DataType Double
PS C:\>$table2 = New-PowerBITable -Name SampleTable2 -Columns $col3,$col4,$col5,$col6
PS C:\>
PS C:\>$dataset = New-PowerBIDataSet -Name SampleDataSet -Tables $table1,$table2
PS C:\>
PS C:\>Add-PowerBIDataSet -DataSet $dataset
```

This example instantiate a table with two columns and another table with four columns, and instantiate a dataset.
Then, it creates the dataset in PowerBI.

## PARAMETERS

### -Dataset
The dataset to be added.

```yaml
Type: Dataset
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual returns datasets from "My Workspace" by default. With -Workspace or -WorkspaceId, datasets under the workspace assigned to the caller are returned; Organization returns all datasets within a tenant (must be an administrator to initiate). Individual is the default.

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

### -Workspace
Workspace to add the dataset.

```yaml
Type: Workspace
Parameter Sets: Workspace
Aliases: Group

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WorkspaceId
Workplace Id to add the dataset.

```yaml
Type: Guid
Parameter Sets: Dataset
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

### Microsoft.PowerBI.Common.Api.Workspaces.Workspace

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## NOTES

## RELATED LINKS
