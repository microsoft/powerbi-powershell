---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# Copy-PowerBITile

## SYNOPSIS
Creates a copy of the specified Power BI tile and adds it to a Power BI dashboard.

## SYNTAX

### MyWorkspace (Default)
```
Copy-PowerBITile -DashboardId <Guid> -TileId <Guid> -TargetDashboardId <Guid> [-TargetWorkspaceId <Guid>]
 [-TargetReportId <Guid>] [-TargetDatasetId <Guid>] [-PositionConflictAction <PositionConflictAction>]
 [<CommonParameters>]
```

### WorkspaceId
```
Copy-PowerBITile -WorkspaceId <Guid> -DashboardId <Guid> -TileId <Guid> -TargetDashboardId <Guid>
 [-TargetWorkspaceId <Guid>] [-TargetReportId <Guid>] [-TargetDatasetId <Guid>]
 [-PositionConflictAction <PositionConflictAction>] [<CommonParameters>]
```

### WorkspaceObject
```
Copy-PowerBITile -Workspace <Workspace> -DashboardId <Guid> -TileId <Guid> -TargetDashboardId <Guid>
 [-TargetWorkspaceId <Guid>] [-TargetReportId <Guid>] [-TargetDatasetId <Guid>]
 [-PositionConflictAction <PositionConflictAction>] [<CommonParameters>]
```

## DESCRIPTION
Creates a copy of the specified tile and adds it to a dashboard.
If target report ID and target dataset ID are not specified, the following can occur:
* When a tile copy is created within the same workspace, the report and dataset links will be copied from the source tile.
* When copying a tile to a different workspace, the report and dataset links will be rested, and the tile will be broken.

## EXAMPLES

### Example 1
```powershell
PS C:\> Copy-PowerBITile -DashboardId cff24b2e-faa8-4683-8ecb-2c50e7d2cc7a -TileId e297e105-be30-4482-8531-152cdf289ac6 -TargetDashboardId 8f88d7ab-49e7-41e0-979b-28f063056daa -targetWorkspaceId 166bc04e-da57-426b-b7b4-d24d0e3e5587 -TargetReportId 1fb4359e-9356-4193-9965-a9472a0051b8 -TargetDatasetId a96cd411-4562-4eba-ba2a-42fee8425a87
```

Creates a copy of the tile with the ID "e297e105-be30-4482-8531-152cdf289ac6" from a dashboard with ID "cff24b2e-faa8-4683-8ecb-2c50e7d2cc7a" and adds it to the dashboard with ID "8f88d7ab-49e7-41e0-979b-28f063056daa" in the workspace with ID "166bc04e-da57-426b-b7b4-d24d0e3e5587" and links it to the report with the ID "1fb4359e-9356-4193-9965-a9472a0051b8" and the dataset with ID "a96cd411-4562-4eba-ba2a-42fee8425a87" in the target workspace.

## PARAMETERS

### -DashboardId
The ID of the dashboard where source tile is located.

```yaml
Type: Guid
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
Type: PositionConflictAction
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetDashboardId
The ID of the dashboard where tile copy should be added.

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

### -TargetDatasetId
Optional parameter to rebind the copied tile to a different dataset.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: TargetModelId

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetReportId
Optional parameter to rebind the copied tile to a different report.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetWorkspaceId
Optional parameter for specifying the target workspace ID. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'. Empty string indicates tile will be copied within the same workspace.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: TargetGroupId

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TileId
The ID of the tile that should be copied

```yaml
Type: Guid
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
The ID of the workspace where the source dashboard is located. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'.

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
