---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Remove-OnPremisesDataGatewayClusterDatasourceUser

## SYNOPSIS
Removes the specified user from the specified datasource of the specified cluster

## SYNTAX

```
Remove-OnPremisesDataGatewayClusterDatasourceUser [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -GatewayClusterDatasourceId <Guid> -UserId <String> [<CommonParameters>]
```

## DESCRIPTION
Removes the specified user from the specified datasource of the specified cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> Remove-OnPremisesDataGatewayClusterDatasourceUser -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -GatewayClusterDatasourceId 64C574B7-86C6-4560-B710-40AC18990804 -UserId testEmail@tenant.com
```

Removes access of 'testUpn@tenant.com' for the datasource.

## PARAMETERS

### -GatewayClusterDatasourceId
{{Fill GatewayClusterDatasourceId Description}}

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: DatasourceId, Datasource

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

### -UserId
The user to remove. This should be either a PrincipalObjectId (GUID type) or match the Identifier/UserEmailAddress value from Get-OnPremisesDataGatewayClusterDatasourceUsers

```yaml
Type: String
Parameter Sets: (All)
Aliases: UserEmailAddress, EmailAddress, PrincipalObjectId, User

Required: True
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
