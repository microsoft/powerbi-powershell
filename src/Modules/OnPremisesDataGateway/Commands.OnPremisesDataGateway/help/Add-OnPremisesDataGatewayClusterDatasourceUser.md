---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Add-OnPremisesDataGatewayClusterDatasourceUser

## SYNOPSIS
Grants the permissions required to use the specified datasource for the specified user

## SYNTAX

```
Add-OnPremisesDataGatewayClusterDatasourceUser [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -GatewayDatasourceId <Guid> -DatasourceUserAccessRight <String> [-DisplayName <String>] [-Identifier <String>]
 [-PrincipalType <String>] [-EmailAddress <String>] [<CommonParameters>]
```

## DESCRIPTION
Grants the permissions required to use the specified datasource for the specified user

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -DatasourceUserAccessRight
{{Fill DatasourceUserAccessRight Description}}

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

### -DisplayName
{{Fill DisplayName Description}}

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

### -EmailAddress
{{Fill EmailAddress Description}}

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

### -GatewayDatasourceId
{{Fill GatewayDatasourceId Description}}

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: DatasourceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Identifier
{{Fill Identifier Description}}

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

### -PrincipalType
{{Fill PrincipalType Description}}

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
