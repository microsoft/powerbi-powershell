---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version:
schema: 2.0.0
---

# Restore-PowerBIWorkspace

## SYNOPSIS
Restores a deleted Power BI workspace.

## SYNTAX

### Properties (Default)
```
Restore-PowerBIWorkspace [-Scope <PowerBIUserScope>] -Id <Guid> [-RestoredName <String>]
 -UserPrincipalName <String> [<CommonParameters>]
```

### Workspace
```
Restore-PowerBIWorkspace [-Scope <PowerBIUserScope>] [-RestoredName <String>] -UserPrincipalName <String>
 -Workspace <Group> [<CommonParameters>]
```

## DESCRIPTION
Restores a deleted Power BI workspace with the user matching the provided email address as the owner.
You must have logged in previously before using Login-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Restore-PowerBIWorkspace -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -Name "TestWorkspace" -UserEmailAddress "john@contoso.com"
```

Restores the workspace with the given ID, assigns the user with the given email address as the owner, and updates the name of the workspace to the given name.

## PARAMETERS

### -Id
The ID of the workspace to restore.

```yaml
Type: Guid
Parameter Sets: Properties
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
Indicates scope of the call. Individual returns only workspaces assigned to them; Organization returns all workspaces within a tenant (must be an administrator to initiate). Individual is the default.

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

### -UserPrincipalName
User Principal Name (or UPN, commonly their email address) for the user who will become the new owner of the restored workspace.

```yaml
Type: String
Parameter Sets: (All)
Aliases: UserEmailAddress

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
The workspace entity to be restored.

```yaml
Type: Group
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
