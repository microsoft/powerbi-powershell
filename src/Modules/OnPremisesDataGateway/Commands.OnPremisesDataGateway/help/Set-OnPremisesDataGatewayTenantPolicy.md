---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Set-OnPremisesDataGatewayTenantPolicy

## SYNOPSIS
Set the gateway creation policy for the tenant

## SYNTAX

```
Set-OnPremisesDataGatewayTenantPolicy [-ResourceGatewayInstallPolicy <PolicyType>]
 [-PersonalGatewayInstallPolicy <PolicyType>] [<CommonParameters>]
```

## DESCRIPTION
Set the gateway creation policy for the tenant

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-OnPremisesDataGatewayTenantPolicy -ResourceGatewayInstallPolicy Open
```

Allow users to create on-premises data gateways on the current user's tenant.

## PARAMETERS

### -PersonalGatewayInstallPolicy
The personal gateway creation tenant policy

```yaml
Type: PolicyType
Parameter Sets: (All)
Aliases:
Accepted values: None, Open, Restricted

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ResourceGatewayInstallPolicy
The on-premises data gateway creation tenant policy

```yaml
Type: PolicyType
Parameter Sets: (All)
Aliases:
Accepted values: None, Open, Restricted

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

### System.Void

## NOTES

## RELATED LINKS
