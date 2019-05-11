---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version:
schema: 2.0.0
---

# New-PowerBIWorkspace

## SYNOPSIS
Creates a new Power BI workspace.

## SYNTAX

```
New-PowerBIWorkspace -Name <String> [-Type <NewWorkspaceType>] [<CommonParameters>]
```

## DESCRIPTION
Creates a new Power BI workspace with the current user as the workspace administrator.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-PowerBIWorkspace -Name "New Workspace"
```

Creates a new Power BI workspace called "New Workspace" and adds the current user as the workspace administrator.

## PARAMETERS

### -Name
The name to give to the new workspace. If the name matches another workspace in the organization, the operation will fail.

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

### -Type
Type of workspace to be created. Options are Workspace or Group. Default is Workspace.

```yaml
Type: NewWorkspaceType
Parameter Sets: (All)
Aliases: WorkspaceType

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
