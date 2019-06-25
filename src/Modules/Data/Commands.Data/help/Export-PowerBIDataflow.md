---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.data/export-powerbidataflow?view=powerbi-ps
schema: 2.0.0
---

# Export-PowerBIDataflow

## SYNOPSIS
Exports a Power BI dataflow to the .pbix file format.

## SYNTAX

```
Export-PowerBIDataflow [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>] -OutFile <String>
 [<CommonParameters>]
```

## DESCRIPTION
Saves a Power BI dataflow from the Power BI service into a .json file that can be imported back to Power BI service.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Export-PowerBIDataflow -Id 9b519499-5ba1-4f1c-878b-be3a69f1791f -OutFile .\Sales.json
```

Saves dataflow with ID 9b519499-5ba1-4f1c-878b-be3a69f1791f into a file named Sales.json in your current working directory ($PWD).

## PARAMETERS

### -Id
ID of the dataflow to export.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: DataflowId

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -OutFile
Output file to save the exported dataflow to. Path must not already exist.

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

### -Scope
Indicates scope of the call. With -WorkspaceId, dataflow under the workspace assigned to the caller is exported; Individual will work only with -WorkspaceId parameter. Organization exports the requested dataflow from within a tenant (must be an administrator to initiate). Individual is the default.

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
ID of the workspace containing the Power BI dataflow to export.

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
