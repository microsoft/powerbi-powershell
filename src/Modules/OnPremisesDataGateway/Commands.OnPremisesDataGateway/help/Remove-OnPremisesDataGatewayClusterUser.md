---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Remove-OnPremisesDataGatewayClusterUser

## SYNOPSIS
Remove user from gateway cluster

## SYNTAX

```
Remove-OnPremisesDataGatewayClusterUser [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -PrincipalObjectId <Guid> [<CommonParameters>]
```

## DESCRIPTION
Remove user from gateway cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> $userToRemove = $(Get-AzureADUser -ObjectId "testUpn@tenant.com").ObjectId
PS C:\> Remove-OnPremisesDataGatewayClusterUser -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -PrincipalObjectId $userToRemove
```

Removes the user "testUpn@tenant.com" from the gateway cluster.

## PARAMETERS

### -GatewayClusterId
Gateway cluster where the user should be removed

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: Cluster, Id

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PrincipalObjectId
Azure Active Directory (AAD) principal object ID (i.e. user ID) to add to the gateway cluster

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: User

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Power BI scope to run the command

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

### System.Void

## NOTES

## RELATED LINKS
