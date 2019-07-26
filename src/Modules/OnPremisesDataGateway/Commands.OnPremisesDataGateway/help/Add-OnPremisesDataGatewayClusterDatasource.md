---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Add-OnPremisesDataGatewayClusterDatasource

## SYNOPSIS
Creates a new datasource on the specified cluster

## SYNTAX

```
Add-OnPremisesDataGatewayClusterDatasource [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -ConnectionDetails <String> -DatasourceName <String> -DatasourceType <String> [-Annotation <String>]
 [-SingleSignOnType <String>] [<CommonParameters>]
```

## DESCRIPTION
Creates a new datasource on the specified cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

This example adds a new datasource for the cluster on SQL datasource types

## PARAMETERS

### -Annotation
{{Fill Annotation Description}}

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

### -ConnectionDetails
{{Fill ConnectionDetails Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatasourceName
{{Fill DatasourceName Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatasourceType
{{Fill DatasourceType Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
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

### -SingleSignOnType
{{Fill SingleSignOnType Description}}

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS
