---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Set-PowerBIDataset

## SYNOPSIS
Updates the properties for the specified Power BI dataset.

## SYNTAX

### DatasetId (Default)
```
Set-PowerBIDataset -Id <Guid> -TargetStorageMode <DatasetStorageMode> [-WorkspaceId <Guid>]
 [<CommonParameters>]
```

### ObjectAndId
```
Set-PowerBIDataset -Id <Guid> -TargetStorageMode <DatasetStorageMode> [-Workspace <Workspace>]
 [<CommonParameters>]
```

### DatasetName
```
Set-PowerBIDataset -Name <String> -TargetStorageMode <DatasetStorageMode> [-WorkspaceId <Guid>]
 [<CommonParameters>]
```

### ObjectAndName
```
Set-PowerBIDataset -Name <String> -TargetStorageMode <DatasetStorageMode> [-Workspace <Workspace>]
 [<CommonParameters>]
```

## DESCRIPTION
Updates the properties for the specified Power BI dataset.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-PowerBIDataset -Id c47f6cff-70de-4837-a094-93a6f26e20bf -TargetStorageMode PremiumFiles
```

Updates the specified datset with target storage mode PremiumFiles.

## PARAMETERS

### -Id
ID of the dataset

```yaml
Type: Guid
Parameter Sets: DatasetId, ObjectAndId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the dataset

```yaml
Type: String
Parameter Sets: DatasetName, ObjectAndName
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TargetStorageMode
Target storage mode of the dataset

```yaml
Type: DatasetStorageMode
Parameter Sets: (All)
Aliases:
Accepted values: Abf, PremiumFiles

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace containing the dataset

```yaml
Type: Workspace
Parameter Sets: ObjectAndId, ObjectAndName
Aliases: Group

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WorkspaceId
ID of the workspace containing the dataset

```yaml
Type: Guid
Parameter Sets: DatasetId, DatasetName
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

### System.String
Microsoft.PowerBI.Common.Api.Workspaces.Workspace

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## NOTES

## RELATED LINKS
