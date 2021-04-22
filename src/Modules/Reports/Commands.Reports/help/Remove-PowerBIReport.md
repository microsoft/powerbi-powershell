---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.reports/remove-powerbireport?view=powerbi-ps
schema: 2.0.0
---

# Remove-PowerBIReport

## SYNOPSIS
Deletes a Power BI report.

## SYNTAX

### MyWorkspace (Default)
```
Remove-PowerBIReport -Id <Guid> [<CommonParameters>]
```

### WorkspaceId
```
Remove-PowerBIReport -Id <Guid> -WorkspaceId <Guid> [<CommonParameters>]
```

### Workspace
```
Remove-PowerBIReport -Id <Guid> -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Removes a Power BI report from a workspace.
If no workspace is specified, your personal (My Workspace) is used.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-PowerBIReport -Id 12345-abc56-jkl56-700a0 -WorkspaceId ccd01-bif87-abc12-34efg
```

Deletes a report with the GUID, '12345-abc56-jkl56-700a0', from the Workspace with ID of 'ccd01-bif87-abc12-34efg'. 

### Example 2
```powershell
PS C:\> Remove-PowerBIReport -Id 12345-abc56-jkl56-700a0
```

Deletes a report with the GUID of '12345-abc56-jkl56-700a0' from your personal workspace.

## PARAMETERS

### -Id
Id of the report to be deleted.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: ReportId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace object that contains the report to be deleted.

```yaml
Type: Workspace
Parameter Sets: Workspace
Aliases: Group

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
Id of the workspace that contains the report to be deleted.

```yaml
Type: Guid
Parameter Sets: WorkspaceId
Aliases: GroupId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
