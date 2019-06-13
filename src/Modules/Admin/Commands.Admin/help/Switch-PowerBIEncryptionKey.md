---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Switch-PowerBIEncryptionKey

## SYNOPSIS
Switch the encryption key for Power BI workspaces assigned to a capacity.

## SYNTAX

```
Switch-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [<CommonParameters>]
```

## DESCRIPTION
Rotate the customer owned key for the tenant.
Make sure the current encryption key and new version of key is valid.
Grant wrap and unwrap key permissions for Power BI service in the Azure Key Vault.
	
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```
PS C:\> Switch-PowerBIEncryptionKey -Name 'Contoso Sales' -KeyVaultKeyUri 'https://contoso-vault2.vault.azure.net/keys/ContosoKeyVault/b2ab4ba1c7b341eea5ecaaa2wb54c4d2'
```

## PARAMETERS

### -KeyVaultKeyUri
Uri to the version of the "Azure Key Vault" key to be used. Only supports 4096 bytes key.

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

### -Name
The name of the encryption key.

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

### Microsoft.PowerBI.Common.Api.Encryption.EncryptionKey

## NOTES

## RELATED LINKS
