---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.workspaces/add-powerbiworkspaceuser?view=powerbi-ps
schema: 2.0.0
---

# Add-PowerBIWorkspaceUser

## SYNOPSIS
Gives permissions to a specified user to access a Power BI workspace. Organization scope only allows requests with user principle.

## SYNTAX

### UserEmailWithId (Default)
```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -Id <Guid> -UserPrincipalName <String>
 -AccessRight <WorkspaceUserAccessRight> [<CommonParameters>]
```

### PrincipalTypeWithId
```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -Id <Guid> -AccessRight <WorkspaceUserAccessRight>
 -PrincipalType <WorkspaceUserPrincipalType> -Identifier <String> [<CommonParameters>]
```

### UserEmailWithWorkspace
```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -UserPrincipalName <String>
 -AccessRight <WorkspaceUserAccessRight> -Workspace <Workspace> [<CommonParameters>]
```

### PrincipalTypeWithWorkspace
```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -AccessRight <WorkspaceUserAccessRight>
 -Workspace <Workspace> -PrincipalType <WorkspaceUserPrincipalType> -Identifier <String> [<CommonParameters>]
```

## DESCRIPTION
Grants permissions to a specified user to access a Power BI workspace using the provided inputs and scope specified.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.

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
Accepted values: Member, Admin, Contributor, Viewer

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the workspace the user should be added to.

```yaml
Type: Guid
Parameter Sets: UserEmailWithId, PrincipalTypeWithId
Aliases: GroupId, WorkspaceId

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Identifier
Identifier of the principal to add to the group. For Apps and Groups, this will be their object identifier (GUID). For users, this can be an email address.

```yaml
Type: String
Parameter Sets: PrincipalTypeWithId, PrincipalTypeWithWorkspace
Aliases: PrincipalId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PrincipalType
The type of the principal to add to the group.

```yaml
Type: WorkspaceUserPrincipalType
Parameter Sets: PrincipalTypeWithId, PrincipalTypeWithWorkspace
Aliases:
Accepted values: App, Group, User

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual operates against only workspaces assigned to the caller; Organization operates against all workspaces within a tenant (must be an administrator to initiate). Individual is the default.

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
Parameter Sets: UserEmailWithId, UserEmailWithWorkspace
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
Parameter Sets: UserEmailWithWorkspace, PrincipalTypeWithWorkspace
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

### System.Object

## NOTES

## RELATED LINKS
