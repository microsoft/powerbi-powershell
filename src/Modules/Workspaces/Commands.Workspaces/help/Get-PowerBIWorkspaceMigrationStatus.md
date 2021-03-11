---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.workspaces/get-powerbiworkspacemigrationstatus?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIWorkspaceMigrationStatus

## SYNOPSIS
Gets Power BI workspace migration status.

## SYNTAX

### Workspace (Default)
```
Get-PowerBIWorkspaceMigrationStatus -Workspace <Workspace> [<CommonParameters>]
```

### Id
```
Get-PowerBIWorkspaceMigrationStatus -Id <Guid> [<CommonParameters>]
```

## DESCRIPTION
Gets Power BI workspace migration status.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIWorkspaceMigrationStatus -Id "3244f1c1-01cf-457f-9383-6035e4950fdc"
```

## PARAMETERS

### -Id
ID of the workspace.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: GroupId, WorkspaceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
The workspace entity.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.PowerBI.Api.V2.Models.WorkspaceLastMigrationStatus

## NOTES

## RELATED LINKS
