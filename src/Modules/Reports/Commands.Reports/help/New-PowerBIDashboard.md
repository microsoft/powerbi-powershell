---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# New-PowerBIDashboard

## SYNOPSIS
Creates a new empty Power BI dashboard.

## SYNTAX

### NameOnly (Default)
```
New-PowerBIDashboard -Name <String> [<CommonParameters>]
```

### NameAndIds
```
New-PowerBIDashboard -Name <String> -WorkspaceId <Guid> [<CommonParameters>]
```

### NameAndObject
```
New-PowerBIDashboard -Name <String> -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Creates a new empty dashboard in "My Workspace" or in a specified workspace.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-PowerBIDashboard -Name "New Dashboard"
```

Creates a new dashboard called "New Dashboard" in "My Workspace".

### Example 2
```powershell
PS C:\> New-PowerBIDashboard -Name "New Dashboard" -WorkspaceId 084d15de-4c96-4a66-97b4-c60914a0973c
```

Creates a new dashboard called "New Dashboard" in an existing workspace with ID "084d15de-4c96-4a66-97b4-c60914a0973c".

## PARAMETERS

### -Name
Name of the dashboard.

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

### -Workspace
Workspace object, as returned by the Get-PowerBIWorkspace cmdlet, in which the dashboard should be created.

```yaml
Type: Workspace
Parameter Sets: NameAndObject
Aliases: Group

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
ID of the workspace in which the dashboard should be created.

```yaml
Type: Guid
Parameter Sets: NameAndIds
Aliases: GroupId

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

### Microsoft.PowerBI.Common.Api.Reports.Dashboard

## NOTES

## RELATED LINKS

