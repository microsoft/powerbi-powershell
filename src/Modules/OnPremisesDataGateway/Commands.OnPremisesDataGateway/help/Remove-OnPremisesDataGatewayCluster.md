---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Remove-OnPremisesDataGatewayCluster

## SYNOPSIS
Remove a gateway cluster

## SYNTAX

```
Remove-OnPremisesDataGatewayCluster [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid> [<CommonParameters>]
```

## DESCRIPTION
Remove a gateway cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-OnPremisesDataGatewayCluster -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77
```

Remove the cluster with ID DC8F2C49-5731-4B27-966B-3DB5094C2E77

## PARAMETERS

### -GatewayClusterId
The cluster ID of the gateway to remove

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: Cluster

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
