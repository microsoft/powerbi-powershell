---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Remove-OnPremisesDataGatewayClusterMember

## SYNOPSIS
Remove gateway from gateway cluster

## SYNTAX

```
Remove-OnPremisesDataGatewayClusterMember [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -MemberGatewayId <Guid> [<CommonParameters>]
```

## DESCRIPTION
Remove gateway from gateway cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-OnPremisesDataGatewayClusterMember -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -MemberGatewayId E407A364-3A89-4E21-8791-C108DB41E75A
```

Remove the gateway with ID E407A364-3A89-4E21-8791-C108DB41E75A from the
cluster with ID DC8F2C49-5731-4B27-966B-3DB5094C2E77

## PARAMETERS

### -GatewayClusterId
The cluster where the gateway to be removed is a member

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

### -MemberGatewayId
The gateway to remove from the cluster

```yaml
Type: Guid
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Security Scope to run the command. This would determine if you are running this command in the scope of a Tenant/Service admin or a Gateway Admin

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
