---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Get-OnPremisesDataGatewayCluster

## SYNOPSIS
Get clusters for the current user

## SYNTAX

```
Get-OnPremisesDataGatewayCluster [-Scope <PowerBIUserScope>] [-GatewayClusterId <Guid>] [<CommonParameters>]
```

## DESCRIPTION
Get clusters that match the cluster ID for the current user. If no cluster ID
is specified, all clusters will be returned.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-OnPremisesDataGatewayCluster
```

Get all clusters details for the current user

### Example 2
```powershell
PS C:\> Get-OnPremisesDataGatewayCluster -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77
```

Get cluster details for the specific cluster "DC8F2C49-5731-4B27-966B-3DB5094C2E77"

## PARAMETERS

### -GatewayClusterId
Get a specific cluster

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: Cluster, Id

Required: False
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

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Gateways.Entities.GatewayCluster, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
