---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# New-PowerBIReport

## SYNOPSIS
Deploys a PowerBI report

## SYNTAX

### WorkspaceId
```
New-PowerBIReport [-Path] <String> [[-Name] <String>] [[-WorkspaceId] <Guid>] [[-ConflictAction] <String>]
 [<CommonParameters>]
```

### Workspace
```
New-PowerBIReport [-Path] <String> [[-Name] <String>] [[-Workspace] <Workspace>] [[-ConflictAction] <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Loads a PowerBI report from a pbix file and deploys it to the PowerBI service.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```
PS C:\> New-PowerBIReport -Path '.\report.pbix' -Name 'Report'
```

Adds the report to the personal Workspace.

### Example 1
```
PS C:\> New-PowerBIReport -Path '.\report.pbix' -Name 'Report' -Workspace ( Get-PowerBIWorkspace -Name 'Team Workspace' )
```

Adds the report to the Team Workspace.

## PARAMETERS

### -ConflictAction
{{Fill ConflictAction Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Abort, CreateOrOverwrite, Ignore, Overwrite

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
The report name. If not set, the file name will be used.

```yaml
Type: String
Parameter Sets: (All)
Aliases: ReportName

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
The path to the pbix file.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
The workspace to deploy the report to.

```yaml
Type: Workspace
Parameter Sets: Workspace
Aliases: Group

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
The id of the workspace to deploy the report to

```yaml
Type: Guid
Parameter Sets: WorkspaceId
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

### Keine

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Reports.Report

## NOTES

## RELATED LINKS
