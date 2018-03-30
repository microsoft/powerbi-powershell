---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version:
schema: 2.0.0
---

# Get-PowerBIReport

## SYNOPSIS
Returns a list of Power BI reports.

## SYNTAX

```
Get-PowerBIReport [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI reports with the provided search criteria and scope specified.
You must have logged in previously before using, Login-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIReport
```

Returns a list of all PowerBI reports a user has access to.

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Api.V2.Models.Report, Microsoft.PowerBI.Api, Version=2.0.10.18022, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]

## NOTES

## RELATED LINKS
