---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# New-PowerBIColumn

## SYNOPSIS
Creates new PowerBI column object.

## SYNTAX

```
New-PowerBIColumn -Name <String> -DataType <PowerBIDataType> [<CommonParameters>]
```

## DESCRIPTION
The New-PowerBIColumn cmdlet lets you create new PowerBI column object.

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

### -DataType
A type of the column. Type can be String, Int64, DateTime, Boolean, or Double

```yaml
Type: PowerBIDataType
Parameter Sets: (All)
Aliases:
Accepted values: In64, Double, Boolean, Datetime, String

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
A column name.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Column

## NOTES

## RELATED LINKS
