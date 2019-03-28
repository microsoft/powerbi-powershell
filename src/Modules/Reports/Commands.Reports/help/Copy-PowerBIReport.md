---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# Copy-PowerBIReport

## SYNOPSIS
Creates a copy of the specified report.

## SYNTAX

### Id (Default)
```
Copy-PowerBIReport -Name <String> -Id <Guid> [-WorkspaceId <String>] [-Workspace <Workspace>]
 [-TargetWorkspaceId <String>] [-TargetDatasetId <String>] [<CommonParameters>]
```

### WorkspaceObject
```
Copy-PowerBIReport [-Name <String>] -Report <Report> [-WorkspaceId <String>] [-Workspace <Workspace>]
 [-TargetWorkspaceId <String>] [-TargetDatasetId <String>] [<CommonParameters>]
```

## DESCRIPTION
Creates a copy of the specified report in the same workspace or in a different workspace and rebinds the report to a referenced dataset in the target workspace.

## EXAMPLES

### Example 1
```powershell
PS C:\> Copy-PowerBIReport -Name "Report Copy" -Id "30ca8f24-f628-45f7-a5ac-540c95e9b5e6" -WorkspaceId "00000000-0000-0000-0000-000000000000" -TargetWorkspaceId "6439d4d4-18c4-4762-b755-1f957d55383e" -TargetDatasetId "74f6adb5-93eb-49d8-918c-6df248cb73dd"
```

Creates a copy of the report with id "30ca8f24-f628-45f7-a5ac-540c95e9b5e6" from 'My Workspace' in the target workspace with id "6439d4d4-18c4-4762-b755-1f957d55383e", assigns the report copy the name "Report Copy", and binds it to the dataset with the id "74f6adb5-93eb-49d8-918c-6df248cb73dd".

### Example 2
```powershell
PS C:\> Copy-PowerBIReport -Name "Report Copy" -Id "bd200f64-46f1-4f82-b09f-c7fd6818d67c" -WorkspaceId "6439d4d4-18c4-4762-b755-1f957d55383e" -TargetWorkspaceId "00000000-0000-0000-0000-000000000000" -TargetDatasetId "1b46e4dc-1299-425b-97aa-c10d51f82a06"
```

Creates a copy of the report with id "bd200f64-46f1-4f82-b09f-c7fd6818d67c"" from the workspace with the id "6439d4d4-18c4-4762-b755-1f957d55383e" in 'My Workspace' as the target workspace, assigns the report copy the name "Report Copy", and binds it to the dataset with the id "1b46e4dc-1299-425b-97aa-c10d51f82a06".

### Example 1
```powershell
PS C:\> Copy-PowerBIReport -Name "Report Copy" -Id "30ca8f24-f628-45f7-a5ac-540c95e9b5e6"
```

Creates a copy of the report with id "30ca8f24-f628-45f7-a5ac-540c95e9b5e6" in the same workspace, assigns the report copy the name "Report Copy", and binds it to the dataset that the original report is associated with.


## PARAMETERS

### -Id
The ID of the original report.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: ReportId

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Name
The new report name that will be assigned to the report copy.

```yaml
Type: String
Parameter Sets: Id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: String
Parameter Sets: WorkspaceObject
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Report
The original report object, as obtained by using the Get-PowerBIReport cmdlet.

```yaml
Type: Report
Parameter Sets: WorkspaceObject
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetDatasetId
Optional parameter for specifying the target associated dataset id. If empty, the new report will be associated with the same dataset as the source report.

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

### -TargetWorkspaceId
The ID of the target workspace. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'. Empty string indicates new report will be copied within the same workspace as the source report.

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

### -Workspace
The workspace object, as obtained by using the Get-PowerBIWorkspace cmdlets, where the original report is located. If empty, source workspace is 'My Workspace'.

```yaml
Type: Workspace
Parameter Sets: (All)
Aliases: Group

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkspaceId
The ID of the workspace where the original report is located. If empty, source workspace is 'My Workspace'.

```yaml
Type: String
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

### System.Object

## NOTES

## RELATED LINKS
