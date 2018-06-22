---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.reports/get-powerbitile?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBITile

## SYNOPSIS
Returns a list of Power BI tiles for a dashboard.

## SYNTAX

### List (Default)
```
Get-PowerBITile -DashboardId <Guid> [-Scope <PowerBIUserScope>] [-First <Int32>] [-Skip <Int32>]
 [-WorkspaceId <Guid>] [<CommonParameters>]
```

### Id
```
Get-PowerBITile -DashboardId <Guid> -Id <Guid> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>]
 [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI tiles that match the specified search criteria and scope.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBITile -DashboardId 6b071d0b-7430-4342-a3a6-d5c0fac130e4
```

Returns a list of tiles in the Power BI dashboard with ID 6b071d0b-7430-4342-a3a6-d5c0fac130e4, for a user that has access to the dashboard.

### Example 2
```powershell
PS C:\> Get-PowerBITile -DashboardId 6b071d0b-7430-4342-a3a6-d5c0fac130e4 -Scope Organization
```

Returns a list of tiles in the Power BI dashboard with ID 6b071d0b-7430-4342-a3a6-d5c0fac130e4, for a dashboard in the user's organization.

## PARAMETERS

### -DashboardId
Id of the dashboard to return tiles for.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -First
First (top) list of results.

```yaml
Type: Int32
Parameter Sets: List
Aliases: Top

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Id of the tile to return.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: ImportId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual returns only tiles assigned to the caller; Organization returns all tiles within a tenant (must be an administrator to initiate). Individual is the default.

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

### -Skip
Skips the first set of results.

```yaml
Type: Int32
Parameter Sets: List
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
Workspace Id to filter results to; only tiles that belong to that workspace are shown. Only available when -Scope is Individual.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: GroupId

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

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Reports.Tile, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
