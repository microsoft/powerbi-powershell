---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Set-PowerBIEncryptionKey

## SYNOPSIS
Set the encryption key

## SYNTAX

```
Set-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [<CommonParameters>]
```

## DESCRIPTION
The commandlet will rotate the customer owned key. Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-PowerBIEncryptionKey -Name 'Contoso Sales' -KeyVaultKeyUri 'https://contoso-vault2.vault.azure.net'
```

## PARAMETERS

### -KeyVaultKeyUri
Uri to the version of the "Azure Key Vault" key to be used.
The keyvault key need to adhere to the requirements and recommendations that SQL Azure has for bringing your own key

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
The name of the key to be rotated

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

### System.Object

## NOTES

## RELATED LINKS
