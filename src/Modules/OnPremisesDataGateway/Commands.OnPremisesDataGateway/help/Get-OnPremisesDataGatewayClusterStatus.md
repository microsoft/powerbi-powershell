---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Get-OnPremisesDataGatewayClusterStatus

## SYNOPSIS
Get cluster status

## SYNTAX

```
Get-OnPremisesDataGatewayClusterStatus [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 [<CommonParameters>]
```

## DESCRIPTION
Get cluster status of a particular cluster. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-OnPremisesDataGatewayClusterStatus -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77
```

Get the status of the gateway cluster DC8F2C49-5731-4B27-966B-3DB5094C2E77

## PARAMETERS

### -GatewayClusterId
Cluster to get status of

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

### [Microsoft.PowerBI.Common.Api.Gateways.Entities.GatewayClusterStatusResponse, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]

## NOTES

## RELATED LINKS
