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
Get-PowerBIActivityEvents -StartDateTime <String> -EndDateTime <String> [-ContinuationToken <String>]
 [-Filter <String>] [<CommonParameters>]
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
PS C:\> Get-PowerBIActivityEvents -StartDateTime 2019-08-10T14:35:20 -EndDateTime 2019-08-10T18:25:50 -ContinuationToken %2BRID%3A244SAKlHY7YLAAAAAAAAAA%3D%3D%23RT%3A2%23TRC%3A10%23FPC%3AAQsAAAAAAAAAFwAAAAAAAAA%3D -Filter viewreport
```

## PARAMETERS

### -StartDateTime
Indicates the starting date time of the window of audit event results. It should be in UTC format and also ISO 8601 compliant. Both [StartDateTime](#-startdatetime) and [EndDateTime](#-enddatetime) should be within the same UTC day.

```yaml
Type: String
Parameter Sets: List
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EndDateTime
Indicates the ending date time of the window of audit event results. It should be in UTC format and also ISO 8601 compliant. Both [StartDateTime](#-startdatetime) and [EndDateTime](#-enddatetime) should be within the same UTC day.

```yaml
Type: String
Parameter Sets: List
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContinuationToken
A string token that can be used to retrieive the next chunk of the result set.

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

### -Filter
Filters the results based on a boolean condition. Currently only supporting filterting based on audit event record's 'Activity' type.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.PowerBI.Common.Api.ActivityEvent.ActivityEventResponse

## NOTES

## RELATED LINKS
