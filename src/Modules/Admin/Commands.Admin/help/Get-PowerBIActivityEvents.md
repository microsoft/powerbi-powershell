---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.admin/get-powerbiactivityevents?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIActivityEvents

## SYNOPSIS
Retrieves the audit activity events for a Power BI tenant.

## SYNTAX

```
Get-PowerBIActivityEvents -StartDateTime <String> -EndDateTime <String> [-ActivityType <String>]
 [-User <String>] [-ResultType <OutputType>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves the audit activity events for the calling user's tenant.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIActivityEvents -StartDateTime 2019-08-10T14:35:20 -EndDateTime 2019-08-10T18:25:50
```

### Example 2
```powershell
PS C:\> Get-PowerBIActivityEvents -StartDateTime 2019-08-10T14:35:20 -EndDateTime 2019-08-10T18:25:50 -ActivityType viewreport -User admin@tenant.com -ResultType JsonObject
```

## PARAMETERS

### -ActivityType
Filters the activity records based on this activity type.

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
Specifies the end of a timespan to retrieve audit activity events. It should be in UTC format and ISO 8601 compliant. Both StartDateTime and EndDateTime should be within the same UTC day.

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
Specifies the type of result that is returned by the cmdlet.

```yaml
Type: OutputType
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: JsonString
Accept pipeline input: False
Accept wildcard characters: False
```

### -StartDateTime
Specifies the start of a timespan to retrieve audit activity events. It should be in UTC format and ISO 8601 compliant. Both StartDateTime and EndDateTime should be within the same UTC day.

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

### -User
Filters the activity records based on this user email.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.PowerBI.Common.Api.ActivityEvent.ActivityEventResponse

## NOTES

## RELATED LINKS
