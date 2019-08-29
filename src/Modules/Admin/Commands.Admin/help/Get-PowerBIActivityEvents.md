---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Get-PowerBIActivityEvents

## SYNOPSIS
Retrieves the audit activity events for the tenant.

## SYNTAX

```
Get-PowerBIActivityEvents -StartDateTime <String> -EndDateTime <String> [-ActivityType <String>]
 [-ResultType <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves the audit activity events by the tenant administrator for the organization.

This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIActivityEvents -StartDateTime 2019-08-10T14:35:20 -EndDateTime 2019-08-10T18:25:50
```

### Example 2
```powershell
PS C:\> Get-PowerBIActivityEvents -StartDateTime 2019-08-10T14:35:20 -EndDateTime 2019-08-10T18:25:50 -ActivityType viewreport -ResultType 1
```

## PARAMETERS

### -ActivityType
Filters the results based on a boolean condition. Currently only supporting filterting based on audit event record's 'Activity'

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

### -EndDateTime
Indicates the ending date time of the window of audit event results. It should be in UTC format and also ISO 8601 compliant. Both [StartDateTime](#-startdatetime) and [EndDateTime](#-enddatetime) should be within the same UTC day.

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

### -ResultType
Indicates the type of result that is returned by the cmdlet. Currently the supported values are:

* 0: Result type is string.
* 1: Result type is List of objects.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -StartDateTime
Indicates the starting date time of the window of audit event results. It should be in UTC format and also ISO 8601 compliant. Both [StartDateTime](#-startdatetime) and [EndDateTime](#-enddatetime) should be within the same UTC day.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.PowerBI.Common.Api.ActivityEvent.ActivityEventResponse

## NOTES

## RELATED LINKS
