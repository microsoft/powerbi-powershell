---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Add-PowerBIEncryptionKey

## SYNOPSIS
Adds an encryption key for Power BI workspaces assigned to a capacity.

## SYNTAX

### DefaultAndActivate (Default)
```
Add-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [-Default] [-Activate] [<CommonParameters>]
```

### Default
```
Add-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [-Default] [<CommonParameters>]
```

### Activate
```
Add-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [-Activate] [<CommonParameters>]
```

## DESCRIPTION
Associates an encryption key for Power BI workspaces that is assigned to a premium capacity.
Make sure to grant wrap and unwrap key permissions for Power BI service in the Azure Key Vault.

Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```
PS C:\> Add-PowerBIEncryptionKey -Name 'Contoso Sales' -KeyVaultKeyUri 'https://contoso-vault2.vault.azure.net/keys/ContosoKeyVault/b2ab4ba1c7b341eea5ecaaa2wb54c4d2'
```

## PARAMETERS

### -Activate
Indicates to activate any inactivated capacities to use this key for its encryption

```yaml
Type: SwitchParameter
Parameter Sets: DefaultAndActivate, Activate
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Default
Indicates that this key is set as default for the entire tenant.
Any new capacity creation will inherit this key upon creation.

```yaml
Type: SwitchParameter
Parameter Sets: DefaultAndActivate, Default
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### Microsoft.PowerBI.Common.Api.Encryption.EncryptionKey

## NOTES

## RELATED LINKS

