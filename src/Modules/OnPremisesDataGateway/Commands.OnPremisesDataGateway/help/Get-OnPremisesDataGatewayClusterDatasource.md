---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Get-OnPremisesDataGatewayClusterDatasource

## SYNOPSIS
Returns the specified datasource from the specified cluster

## SYNTAX

```
Get-OnPremisesDataGatewayClusterDatasource [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 [-GatewayClusterDatasourceId <Guid>] [<CommonParameters>]
```

## DESCRIPTION
Returns the specified datasource from the specified cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-OnPremisesDataGatewayClusterDatasource -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -
```

Get all datasources on the cluster

## PARAMETERS

### -GatewayClusterDatasourceId
{{Fill GatewayClusterDatasourceId Description}}

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: DatasourceId, Datasource

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GatewayClusterId
{{Fill GatewayClusterId Description}}

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

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
