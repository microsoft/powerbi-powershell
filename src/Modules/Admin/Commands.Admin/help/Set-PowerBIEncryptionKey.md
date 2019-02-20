---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Set-PowerBIEncryptionKey

## SYNOPSIS
Rotate the encryption key

## SYNTAX

```
Set-PowerBIEncryptionKey -Name <String> -KeyVaultKeyUri <String> [<CommonParameters>]
```

## DESCRIPTION
The commandlet will rotate the customer owned key. Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
Requires TenantReadWriteAll permission

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-PowerBIEncryptionKey -Name 'testName' -KeyVaultKeyUri 'Uri'
```

## PARAMETERS

### -KeyVaultKeyUri
The uri of the KeyVaultKeyUri parameter is an Uri to the version of the AzureKeyVault key to be used.

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
