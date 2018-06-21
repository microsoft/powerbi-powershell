---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# Export-PowerBIReport

## SYNOPSIS
Exports a Power BI report to the .pbix file format.

## SYNTAX

```
Export-PowerBIReport [-WorkspaceId <Guid>] -Id <Guid> -OutFile <String> [<CommonParameters>]
```

## DESCRIPTION
Saves a Power BI report from the Power BI service into a .pbix file that can be loaded by the Power BI Desktop or reuploaded to Power BI service.
You must have logged in previously before, using Connect-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Export-PowerBIReport -Id 9b519499-5ba1-4f1c-878b-be3a69f1791f -OutFile .\Sales.pbix
```

Saves report with ID 9b519499-5ba1-4f1c-878b-be3a69f1791f into a file named Sales.pbix in your current working directory ($PWD).

## PARAMETERS

### -Id
Id of the report to export.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: ReportId

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OutFile
Output file to save the exported report to. Path must not already exist.

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

### -WorkspaceId
Workspace ID containing the Power BI report to export.

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

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
