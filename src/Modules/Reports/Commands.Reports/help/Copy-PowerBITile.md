---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# Copy-PowerBITile

## SYNOPSIS
Creates a copy of the specified tile and adds it to a dashboard.

## SYNTAX

### MyWorkspace (Default)
```
Copy-PowerBITile -DashboardId <String> -TileId <String> -TargetDashboardId <String>
 [-TargetWorkspaceId <String>] [-TargetReportId <String>] [-TargetDatasetId <String>]
 [-PositionConflictAction <String>] [<CommonParameters>]
```

### WorkspaceId
```
Copy-PowerBITile -WorkspaceId <Guid> -DashboardId <String> -TileId <String> -TargetDashboardId <String>
 [-TargetWorkspaceId <String>] [-TargetReportId <String>] [-TargetDatasetId <String>]
 [-PositionConflictAction <String>] [<CommonParameters>]
```

### WorkspaceObject
```
Copy-PowerBITile -Workspace <Workspace> -DashboardId <String> -TileId <String> -TargetDashboardId <String>
 [-TargetWorkspaceId <String>] [-TargetReportId <String>] [-TargetDatasetId <String>]
 [-PositionConflictAction <String>] [<CommonParameters>]
```

## DESCRIPTION
Creates a copy of the specified tile and adds it to a dashboard.
If target report id and target dataset id are not specified, the following can occur:
* When a tile copy is created within the same workspace, the report and dataset links will be copied from the source tile.
* When copying a tile to a different workspace, the report and dataset links will be rested, and the tile will be broken.
## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -DashboardId
The id of the dashboard where source tile is located.

```yaml
Type: String
Parameter Sets: (All)
Aliases: DashboardKey

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PositionConflictAction
Optional parameter for specifying the action in case of position conflict. The default is 'Tail'.

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

### -TargetDashboardId
The id of the dashboard where tile copy should be added.

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

### -TargetDatasetId
Optional parameter to rebind the tile copy to a different dataset.

```yaml
Type: String
Parameter Sets: (All)
Aliases: TargetModelId

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetReportId
Optional parameter to rebind the tile copy to a different report.

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

### -TargetWorkspaceId
Optional parameter for specifying the target workspace id. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'. Empty string indicates tile will be copied within the same workspace.

```yaml
Type: String
Parameter Sets: (All)
Aliases: TargetGroupId

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TileId
The id of the tile that should be copied

```yaml
Type: String
Parameter Sets: (All)
Aliases: TileKey

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace object, as returned by the Get-PowerBIWorkspace cmdlet, where the source dashboard is located.

```yaml
Type: Workspace
Parameter Sets: WorkspaceObject
Aliases: Group

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
The id of the workspace where the source dashboard is located. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'.

```yaml
Type: Guid
Parameter Sets: WorkspaceId
Aliases: GroupId

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

### Microsoft.PowerBI.Common.Api.Reports.Dashboard

## NOTES

## RELATED LINKS
