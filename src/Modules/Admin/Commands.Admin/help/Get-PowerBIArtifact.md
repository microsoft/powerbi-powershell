---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Get-PowerBIArtifact

## SYNOPSIS
Retrieves artifacts belonging to Power BI workspaces.

## SYNTAX

### List
```
Get-PowerBIArtifact [-First <Int32>] [-Skip <Int32>] [<CommonParameters>]
```

### All
```
Get-PowerBIArtifact [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI workspaces with information about reports, dashboards, datasets, dataflows and workbooks.
By default (without -First parameter) it shows the first 100 workspaces. Use -First and -Skip to fetch more workspaces or use -All to return all workspaces.

Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

Returns the first 100 Power BI workspaces for the organization.

## PARAMETERS

### -All
Indicates to show all the workspaces. -First and -Skip cannot be used with this parameter.

```yaml
Type: SwitchParameter
Parameter Sets: All
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -First
First (top) list of results. This value defaults to 100.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Workspaces.Workspace, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
