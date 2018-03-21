---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version:
schema: 2.0.0
---

# Set-PowerBIWorkspace

## SYNOPSIS
Updates the Power BI workspace.

## SYNTAX

### Properties (Default)
```
Set-PowerBIWorkspace [-Scope <PowerBIUserScope>] -Id <Guid> [-Name <String>] [-Description <String>]
 [<CommonParameters>]
```

### Workspace
```
Set-PowerBIWorkspace [-Scope <PowerBIUserScope>] -Workspace <Group> [<CommonParameters>]
```

## DESCRIPTION
This cmdlet will update the name or description of a Power BI workspace by using Power BI .NET SDK which calls the Power BI REST API.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-PowerBIWorkspace -Scope Organization -Id "3244f1c1-01cf-457f-9383-6035e4950fdc" -Name "Test Name" -Description "Test Description"
```

If the current user is an admin, this will update the workspace matching the given ID with the given name and description values.

### Example 2
```powershell
PS C:\> $workspaces = Get-PowerBIWorkspace -Scope Organization
PS C:\> $workspace = $workspaces[0]
PS C:\> $workspace.Name = "Test Name"
PS C:\> $workspace.Description = "Test Description"
PS C:\> Set-PowerBIWorkspace -Scope Organization -Workspace $workspace
```

If the current user is an admin, this will update the given workspace object with the given name and description values.

## PARAMETERS

### -Description
The new description to give to the workspace to update.

```yaml
Type: String
Parameter Sets: Properties
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
The object ID of the workspace to update.

```yaml
Type: Guid
Parameter Sets: Properties
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
The new name to give to the workspace to update. If the name matches another workspace in the organization, the update operation will fail.

```yaml
Type: String
Parameter Sets: Properties
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
The level of access requested for workspace entities. Individual is currently not supported.

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
The workspace entity to update. The workspace will be updated with the name and description on the object.

```yaml
Type: Group
Parameter Sets: Workspace
Aliases:

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
