---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.reports/get-powerbireport?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIReport

## SYNOPSIS
Returns a list of Power BI reports.

## SYNTAX

### List (Default)
```
Get-PowerBIReport [-Scope <PowerBIUserScope>] [-Filter <String>] [-First <Int32>] [-Skip <Int32>]
 [<CommonParameters>]
```

### Id
```
Get-PowerBIReport -Id <Guid> [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

### Name
```
Get-PowerBIReport -Name <String> [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI reports with the provided search criteria and scope specified.
You must have logged in previously before, using Login-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIReport
```

Returns a list of all PowerBI reports a user has access to.

## PARAMETERS

### -Filter
{{Fill Filter Description}}

```yaml
Type: String
Parameter Sets: List
Aliases:

Required: False
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
Aliases: ReportId

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Reports.Report, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
