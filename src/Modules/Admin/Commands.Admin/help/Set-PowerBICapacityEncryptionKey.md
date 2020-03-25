---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Set-PowerBICapacityEncryptionKey

## SYNOPSIS
Updates the encryption key for the Power BI capacity.

## SYNTAX

### KeyNameAndCapacityId (Default)
```
Set-PowerBICapacityEncryptionKey -KeyName <String> -CapacityId <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### KeyNameAndCapacity
```
Set-PowerBICapacityEncryptionKey -KeyName <String> -Capacity <Capacity> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

### KeyAndCapacityId
```
Set-PowerBICapacityEncryptionKey -Key <EncryptionKey> -CapacityId <Guid> [-Scope <PowerBIUserScope>]
 [<CommonParameters>]
```

## DESCRIPTION
Updates the encryption key associated with the Power BI capacity.
Encryption key for the tenant must be exist before calling this cmdlet.

Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-PowerBICapacityEncryptionKey -CapacityId 08d57fce-9e79-49ac-afac-d61765f97f6f -KeyName 'Contoso Sales'
```

## PARAMETERS

### -Capacity
The capacity entity to update.

```yaml
Type: Capacity
Parameter Sets: KeyNameAndCapacity
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -CapacityId
ID of the capacity to update.

```yaml
Type: Guid
Parameter Sets: KeyNameAndCapacityId, KeyAndCapacityId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Key
The encryption key entity to use.

```yaml
Type: EncryptionKey
Parameter Sets: KeyAndCapacityId
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -KeyName
Name of the encryption key to use.

```yaml
Type: String
Parameter Sets: KeyNameAndCapacityId, KeyNameAndCapacity
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

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

### Microsoft.PowerBI.Common.Api.Encryption.EncryptionKey
Microsoft.PowerBI.Common.Api.Capacities.Capacity

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
