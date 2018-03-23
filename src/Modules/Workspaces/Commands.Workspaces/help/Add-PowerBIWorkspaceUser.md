---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version:
schema: 2.0.0
---

# Add-PowerBIWorkspaceUser

## SYNOPSIS
Gives permissions to a given user to access the specified PowerBI workspac

## SYNTAX

```
Add-PowerBIWorkspaceUser [-Scope <PowerBIUserScope>] -Id <Guid> -UserPrincipalName <String>
 -UserAccessRight <String> [<CommonParameters>]
```

## DESCRIPTION
This cmdlet will give permissions to a given user to access the specified PowerBI workspace by using Power BI .NET SDK which calls the Power BI REST API.

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-PowerBIWorkspaceUser -Scope Organization -GroupId 23FCBDBD-A979-45D8-B1C8-6D21E0F4BE50 -UserEmailAddress john@contoso.com -UserAccessRight Admin
```

## PARAMETERS

### -Id
Group or Workspace Id for which user has to be added

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: GroupId, WorkspaceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
The level of access requested for workspace entities. Individual is currently the default value

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

### -UserAccessRight
Permissions to assign to the user

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

### -UserPrincipalName
UserPrincipalName (or UPN, which is mostly same as the email address) for the user whose permissions need to be removed

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
