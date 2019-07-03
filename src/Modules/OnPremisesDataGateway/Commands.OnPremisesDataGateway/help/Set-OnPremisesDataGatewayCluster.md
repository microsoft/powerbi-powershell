---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Set-OnPremisesDataGatewayCluster

## SYNOPSIS
Set attributes of an existing gateway cluster

## SYNTAX

```
Set-OnPremisesDataGatewayCluster [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid> [-Name <String>]
 [-Department <String>] [-Description <String>] [-ContactInformation <String>]
 [-AllowCloudDatasourceRefresh <Boolean>] [-AllowCustomConnectors <Boolean>]
 [-LoadBalancingSelectorType <String>] [<CommonParameters>]
```

## DESCRIPTION
Set attributes of an existing gateway cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> Set-OnPremisesDataGatewayCluster -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -AllowCloudDatasourceRefresh true
```

Allow Power BI cloud datasource refresh on the cluster with ID DC8F2C49-5731-4B27-966B-3DB5094C2E77

## PARAMETERS

### -AllowCloudDatasourceRefresh
If set to true, Power BI cloud datasource refresh is allowed on this cluster.

```yaml
Type: Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AllowCustomConnectors
If set to true, Power BI custom connector based refreshes are allowed on this cluster.

```yaml
Type: Boolean
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContactInformation
Contact information for this cluster.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Department
Department information for this cluster

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Description of this cluster.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GatewayClusterId
Gateway cluster to update

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

### -LoadBalancingSelectorType
Load-balancing type for this cluster.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of this cluster.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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
Default value: Individual
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
