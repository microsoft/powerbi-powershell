---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# Get-PowerBITile

## SYNOPSIS
{{Fill in the Synopsis}}

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
{{Fill in the Description}}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -DashboardId
{{Fill DashboardId Description}}

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
{{Fill First Description}}

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
{{Fill Id Description}}

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

### -Skip
{{Fill Skip Description}}

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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable.
For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None


## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Reports.Tile, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]


## NOTES

## RELATED LINKS
