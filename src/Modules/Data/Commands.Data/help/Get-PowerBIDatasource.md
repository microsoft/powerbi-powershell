---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Get-PowerBIDatasource

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### List (Default)
```
Get-PowerBIDatasource -DatasetId <Guid> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Id
```
Get-PowerBIDatasource -DatasetId <Guid> [-WorkspaceId <Guid>] -Id <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### Name
```
Get-PowerBIDatasource -DatasetId <Guid> [-WorkspaceId <Guid>] -Name <String> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### ObjectAndId
```
Get-PowerBIDatasource -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### ObjectAndName
```
Get-PowerBIDatasource -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### ObjectAndList
```
Get-PowerBIDatasource -Dataset <Dataset> [-WorkspaceId <Guid>] [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
{{Fill in the Description}}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Dataset
{{Fill Dataset Description}}

```yaml
Type: Dataset
Parameter Sets: ObjectAndId, ObjectAndName, ObjectAndList
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DatasetId
{{Fill DatasetId Description}}

```yaml
Type: Guid
Parameter Sets: List, Id, Name
Aliases: DatasetKey

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
{{Fill Id Description}}

```yaml
Type: Guid
Parameter Sets: Id
Aliases: DatasourceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
{{Fill Name Description}}

```yaml
Type: String
Parameter Sets: Name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
{{Fill Scope Description}}

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

### -WorkspaceId
{{Fill WorkspaceId Description}}

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

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
