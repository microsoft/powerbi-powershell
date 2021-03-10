---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.data/export-powerbidataflow?view=powerbi-ps
schema: 2.0.0
---

# Export-PowerBIDataflow

## SYNOPSIS
Exports a Power BI dataflow to the .json file format.

## SYNTAX

### Id (Default)
```
Export-PowerBIDataflow [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>] -OutFile <String>
 [<CommonParameters>]
```

### Dataflow
```
Export-PowerBIDataflow [-WorkspaceId <Guid>] -Dataflow <Dataflow> [-Scope <PowerBIUserScope>] -OutFile <String>
 [<CommonParameters>]
```

## DESCRIPTION
Export a Power BI dataflow from the Power BI service into a .json file that represents a Dataflow object.
For -Scope Individual, user must specify the dataflow's workspace, using the given -WorkspaceId value.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Export-PowerBIDataflow -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375 -Id 9b519499-5ba1-4f1c-878b-be3a69f1791f -OutFile .\Sales.json
```

Export dataflow with ID 9b519499-5ba1-4f1c-878b-be3a69f1791f from a workspace with Id ed451706-2b02-430c-b25c-20c0842c6375 into a file named Sales.json in your current working directory ($PWD).

### Example 2
```powershell
PS C:\> Export-PowerBIDataflow -Id 9b519499-5ba1-4f1c-878b-be3a69f1791f -Scope Organization -OutFile .\Sales.json
```

Export dataflow with ID 9b519499-5ba1-4f1c-878b-be3a69f1791f from within the user's organization into a file named Sales.json in the current working directory ($PWD).

## PARAMETERS

### -Dataflow
Dataflow for exporting.

```yaml
Type: Dataflow
Parameter Sets: Dataflow
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the dataflow to export.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: DataflowId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
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
Indicates scope of the call. Individual works only with the -WorkspaceId parameter, which indicates the Workspace of the requested Dataflow. Organization exports the requested dataflow from within a tenant (must be an administrator to initiate). Individual is the default.

```yaml
Type: PowerBIUserScope
Parameter Sets: (All)
Aliases:
Accepted values: Individual, Organization

Required: False
Position: Named
Default value: Individual
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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Microsoft.PowerBI.Common.Api.Dataflows.Dataflow

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

