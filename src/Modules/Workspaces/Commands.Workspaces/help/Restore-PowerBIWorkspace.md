---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.workspaces/restore-powerbiworkspace?view=powerbi-ps
schema: 2.0.0
---

# Restore-PowerBIWorkspace

## SYNOPSIS
Restores a deleted Power BI workspace.

## SYNTAX

### Id (Default)
```
Restore-PowerBIWorkspace [-Scope <PowerBIUserScope>] -Id <Guid> [-RestoredName <String>]
 -AdminUserPrincipalName <String> [<CommonParameters>]
```

### Workspace
```
Restore-PowerBIWorkspace [-Scope <PowerBIUserScope>] [-RestoredName <String>] -AdminUserPrincipalName <String>
 -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Restores a deleted Power BI workspace with the owner set to the user that matches the specified email address.
Only workspaces in the new workspace experiences preview are supported and only deleted workspaces that haven't reached the data retention policy to be completely removed.
Only -Scope Organization is supported and you must have administrator rights (such as Office 365 Global Administrator or Power BI Service Administrator) to call this command.
You must have logged in previously before using Connect-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Restore-PowerBIWorkspace -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -RestoredName "TestWorkspace" -AdminEmailAddress "john@contoso.com"
```

Restores the workspace with the specified ID, assigns the user with the specified email address as the admin, and updates the name of the workspace to the specified name.

## PARAMETERS

### -AdminUserPrincipalName
User Principal Name (or UPN, commonly an email address) of the user who will become the admin of the restored workspace.

```yaml
Type: String
Parameter Sets: (All)
Aliases: AdminEmailAddress

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the workspace to restore.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: GroupId, WorkspaceId

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -RestoredName
An optional new name to give to the restored workspace.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual is not supported; Organization can restore any deleted workspaces within a tenant (must be an administrator to initiate) unless the workspace met its data retention policy and completed removal. Organization is the default.

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
The workspace entity to be restored.

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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Guid

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
