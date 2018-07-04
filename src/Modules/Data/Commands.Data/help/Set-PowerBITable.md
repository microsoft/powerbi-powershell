---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Set-PowerBITable

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### DatasetId (Default)
```
Set-PowerBITable -Table <Table> -DatasetId <Guid> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Workspace
```
Set-PowerBITable -Table <Table> -DatasetId <Guid> -Dataset <Dataset> [-Workspace <Workspace>]
 [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### WorkspaceId
```
Set-PowerBITable -Table <Table> -DatasetId <Guid> -Dataset <Dataset> [-WorkspaceId <Guid>]
 [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Dataset
```
Set-PowerBITable -Table <Table> -Dataset <Dataset> [-WorkspaceId <Guid>] [-Workspace <Workspace>]
 [-Scope <PowerBIUserScope>] [<CommonParameters>]
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
A dataset where tables stored. You can pass it via pipe.

```yaml
Type: Dataset
Parameter Sets: Workspace, WorkspaceId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: Dataset
Parameter Sets: Dataset
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatasetId
An id of dataset where tables stored.

```yaml
Type: Guid
Parameter Sets: DatasetId, Workspace, WorkspaceId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Only Individual scope is supported.

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

### -Table
Table to be updated.

```yaml
Type: Table
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace to filter the place where table resides.

```yaml
Type: Workspace
Parameter Sets: DatasetId, Workspace, Dataset
Aliases: Group

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WorkspaceId
WorkspaceId to filter the place where table resides.

```yaml
Type: Guid
Parameter Sets: DatasetId, WorkspaceId, Dataset
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
Microsoft.PowerBI.Common.Api.Workspaces.Workspace

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Datasets.Dataset

## NOTES

## RELATED LINKS
