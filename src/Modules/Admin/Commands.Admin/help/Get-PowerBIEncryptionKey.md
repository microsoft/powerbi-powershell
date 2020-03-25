---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Get-PowerBIEncryptionKey

## SYNOPSIS
Retrieves the encryption key for the tenant.

## SYNTAX

```
Get-PowerBIEncryptionKey [-Scope <PowerBIUserScope>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves the encryption key that is added by the tenant administrator for the organization.

Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```
PS C:\> Get-PowerBIEncryptionKey
```

## PARAMETERS

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Encryption.EncryptionKey, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
