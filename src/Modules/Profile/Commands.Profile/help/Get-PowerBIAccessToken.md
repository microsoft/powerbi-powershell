---
external help file: Microsoft.PowerBI.Commands.Profile.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Profile
online version:
schema: 2.0.0
---

# Get-PowerBIAccessToken

## SYNOPSIS
Gets the access token for your current Power BI authenticated session.

## SYNTAX

```
Get-PowerBIAccessToken [-AsString] [<CommonParameters>]
```

## DESCRIPTION
Returns the Power BI access token gathered from logged in Power BI profile.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> $headers = Get-PowerBIAccessToken
PS C:\> Invoke-RestMethod -Headers $headers -Uri 'https://api.powerbi.com/v1.0/myorg/groups'
```

Gets the Power BI access token as a hashtable and passes it to Invoke-RestMethod, as part of the header, to authenticate.

## PARAMETERS

### -AsString
Indicates to return the access token as a string instead of a hashtable. The string contains the authentication type, such as "Bearer".

```yaml
Type: SwitchParameter
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

### System.Object

## NOTES

## RELATED LINKS
