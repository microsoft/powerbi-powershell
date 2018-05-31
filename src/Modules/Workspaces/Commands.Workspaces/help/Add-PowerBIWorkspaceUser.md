---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version:
schema: 2.0.0
---

# Add-PowerBIWorkspaceUser

## SYNOPSIS
Gives permissions to a specified user to access a Power BI workspace.

## SYNTAX

### Id (Default)
```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -Id <Guid> -UserPrincipalName <String>
 -AccessRight <WorkspaceUserAccessRight> [<CommonParameters>]
```

### Workspace
```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -UserPrincipalName <String>
 -AccessRight <WorkspaceUserAccessRight> -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Grants permissions to a specified user to access a Power BI workspace using the provided inputs and scope specified.
You must have logged in previously before, using Login-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-PowerBIWorkspaceUser -Scope Organization -Id 23FCBDBD-A979-45D8-B1C8-6D21E0F4BE50 -UserEmailAddress john@contoso.com -AccessRight Admin
```

## PARAMETERS

### -AccessRight
Permissions to assign to the user.

```yaml
Type: WorkspaceUserAccessRight
Parameter Sets: (All)
Aliases: UserAccessRight
Accepted values: Member, Admin, Contributor

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Workspace or Group Id for which user should be added.

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

### -Scope
Indicates scope of the call. Individual returns only workspaces assigned to the caller; Organization returns all workspaces within a tenant (must be an administrator to initiate). Individual is the default.

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
User Principal Name (or UPN, commonly an email address) for the user whose permissions need to be added.

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
The workspace entity to add the user to.

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

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
