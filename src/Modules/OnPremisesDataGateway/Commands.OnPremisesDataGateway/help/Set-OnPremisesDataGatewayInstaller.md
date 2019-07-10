---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Set-OnPremisesDataGatewayInstaller

## SYNOPSIS
Modify list of users who can install and register new gateways on the tenant

## SYNTAX

```
Set-OnPremisesDataGatewayInstaller -PrincipalObjectIds <String[]> -Operation <OperationType>
 -GatewayType <GatewayType> [<CommonParameters>]
```

## DESCRIPTION
Set which users can install and register new gateways on the tenant.

## EXAMPLES

### Example 1
```powershell
PS C:\> $user1 = $(Get-AzureADUser -ObjectId "testUpn1@tenant.com").ObjectId
PS C:\> $user2 = $(Get-AzureADUser -ObjectId "testUpn2@tenant.com").ObjectId
PS C:\> Set-OnPremisesDataGatewayInstaller -PrincipalObjectIds $user1,$user2 -Operation Add -GatewayType Resource
```

Allow users testUpn1@tenant.com, and testUpn2@tenant.com to register and install an On-premises data gateway in a standard mode.

### Example 2
```powershell
PS C:\> $user1 = $(Get-AzureADUser -ObjectId "testUpn1@tenant.com").ObjectId
PS C:\> $user2 = $(Get-AzureADUser -ObjectId "testUpn2@tenant.com").ObjectId
PS C:\> Set-OnPremisesDataGatewayInstaller -PrincipalObjectIds $user1,$user2 -Operation Remove -GatewayType Resource
```

Users testUpn1@tenant.com, and testUpn2@tenant.com are no longer allowed to register and install an On-premises data gateway in a standard mode.

## PARAMETERS

### -GatewayType
Gateway type the command takes effect on. The value "Resource" below would mean the On-premises data gateway running in a standard mode.

```yaml
Type: GatewayType
Parameter Sets: (All)
Aliases:
Accepted values: Resource, Personal

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Operation
The operation to preform on the permission list.

```yaml
Type: OperationType
Parameter Sets: (All)
Aliases:
Accepted values: None, Add, Remove

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PrincipalObjectIds
List of Azure Active Directory (AAD) principal object IDs (i.e. user IDs) that can configure gateways on the tenant.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: Users

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

### System.Void

## NOTES

## RELATED LINKS
