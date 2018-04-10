---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version:
schema: 2.0.0
---

# Restore-PowerBIWorkspace

## SYNOPSIS
Restores a deleted PowerBI workspace

## SYNTAX

### Properties (Default)
```
Restore-PowerBIWorkspace [-Scope <PowerBIUserScope>] -Id <Guid> [-Name <String>] -EmailAddress <String>
 [<CommonParameters>]
```

### Workspace
```
Restore-PowerBIWorkspace [-Scope <PowerBIUserScope>] [-Name <String>] -EmailAddress <String> -Workspace <Group>
 [<CommonParameters>]
```

## DESCRIPTION
Restores a deleted PowerBI workspace.
Assigns the given user as the new owner.
Updates permissions on the deleted workspace.

## EXAMPLES

### Example 1
```powershell
PS C:\> Restore-PowerBIWorkspace -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -Name "TestWorkspace" -EmailAddress "john@contoso.com"
```

Restores the workspace with the given Id and assigns the user with given email address as owner. Also updates name of restored workspace to the given name.

## PARAMETERS

### -EmailAddress
The email address of the user who is to be added to the restored workspace as owner

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

### -Id
The ID of the workspace to restore

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

### -Name
The new name to be given to the workspace

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

### -Workspace
The workspace entity to be restored

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
