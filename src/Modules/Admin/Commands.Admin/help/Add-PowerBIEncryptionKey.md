---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Add-PowerBIEncryptionKey

## SYNOPSIS
Adds an encryption key for powerbi workspaces in Capacity 

## SYNTAX

### Default (Default)
```
Add-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [-Default] [<CommonParameters>]
```

### DefaultAndActivate
```
Add-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [-Default] [-Activate] [<CommonParameters>]
```

### Activate
```
Add-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [-Activate] [<CommonParameters>]
```

## DESCRIPTION
Adds an encryption key for powerbi workspaces in Capacity. Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
Requires TenantReadWriteAll permission

## EXAMPLES

### Example 1
```
PS C:\> Add-PowerBIEncryptionKey -Name 'testName' -KeyVaultKeyUri 'Uri' -Default -Activate
```

## PARAMETERS

### -Activate
Indicates that all of the capacities that don't have activated the BringYourOwnKey functionality will immediately use this key

```yaml
Type: SwitchParameter
Parameter Sets: DefaultAndActivate, Activate
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Default
Indicates that this key is set as the default for the whole tenant (i.e. any new capacity creation will inherit this key upon creation)

```yaml
Type: SwitchParameter
Parameter Sets: Default, DefaultAndActivate
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeyVaultKeyUri
The uri of the KeyVaultKeyUri parameter is an Uri to the version of the AzureKeyVault key to be used.
The keyvault key need to adhere to the requirements and recommendations that SQL Azure has for BringYourOwnKey

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
The name of the encryption key

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
