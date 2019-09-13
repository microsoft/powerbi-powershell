---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.reports/remove-powerbireport?view=powerbi-ps
schema: 2.0.0
---

# Remove-PowerBIReport

## SYNOPSIS
Deletes a Power BI report from a Workspace.

## SYNTAX

```
Remove-PowerBIReport -Id <Guid> [-WorkspaceId <Guid>] [<CommonParameters>]
```

## DESCRIPTION
Deletes/Removes a Power BI report from a Workspace. The report will not be deleted from everywhere, just in that specific workspace.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-PowerBIReport -Id 12345-abc56-jkl56-700a0 -WorkspaceId ccd01-bif87-abc12-34efg
```

You are deleting a Report with the GUID, '12345-abc56-jkl56-700a0', from the Workspace with Workspace ID of 'ccd01-bif87-abc12-34efg'. 

### Example 2
```powershell
PS C:\> Remove-PowerBIReport -Id 12345-abc56-jkl56-700a0
```

You are deleting a Report with the GUID of '12345-abc56-jkl56-700a0' from whatever Workspace it is in.

## PARAMETERS

### -Id
Report GUID Id that you wish to delete/remove from that workspace

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

### -WorkspaceId
Workspace GUID Id that the report is housed in.

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

### System.Void

## NOTES

## RELATED LINKS
