---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Add-OnPremisesDataGatewayClusterUser

## SYNOPSIS
Add user and associated permissions to cluster

## SYNTAX

```
Add-OnPremisesDataGatewayClusterUser [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -PrincipalObjectId <Guid>
 [-AllowedDataSourceTypes <System.Collections.Generic.List`1[Microsoft.PowerBI.Common.Api.Gateways.Entities.DatasourceType]>]
 -Role <String> [<CommonParameters>]
```

## DESCRIPTION
Add user and associated permissions to cluster

## EXAMPLES

### Example 1
```powershell
PS C:\> $userToAdd = $(Get-AzureADUser -ObjectId "testUpn@tenant.com").ObjectId
PS C:\> $dsTypes = New-Object 'System.Collections.Generic.List[Microsoft.PowerBI.Common.Api.Gateways.Entities.DataSourceType]'
PS C:\> $dsTypes.Add([Microsoft.PowerBI.Common.Api.Gateways.Entities.DataSourceType]::Sql)
PS C:\> Add-OnPremisesDataGatewayClusterUser -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -PrincipalObjectId $userToAdd -AllowedDataSourceTypes $dsTypes -Role ConnectionCreatorWithReshare
```

This example adds the user "testUpn@tenant.com" in the role of ConnectionCreatorWithReshare to the gateway cluster for SQL datasource types.

### Example 2
```powershell
PS C:\> $userToAdd = $(Get-AzureADUser -ObjectId "adminTestUpn@tenant.com").ObjectId
PS C:\> Add-OnPremisesDataGatewayClusterUser -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -PrincipalObjectId $userToAdd -AllowedDataSourceTypes $null -Role Admin
```

This example adds the user "adminTestUpn@tenant.com" in the role of Admin to the gateway cluster for all datasource types.
Note, the AllowedDataSourceTypes must be null when the role is admin (implying all datasource types are allowed).

## PARAMETERS

### -AllowedDataSourceTypes
Datasource types that are allowed for this user/role combination. This must be null if the role is admin.

```yaml
Type: System.Collections.Generic.List`1[Microsoft.PowerBI.Common.Api.Gateways.Entities.DatasourceType]
Parameter Sets: (All)
Aliases:
Accepted values: Sql, AnalysisServices, SAPHana, File, Folder, Oracle, Teradata, SharePointList, Web, OData, DB2, MySql, PostgreSql, Sybase, Extension, SAPBW, AzureTables, AzureBlobs, Informix, ODBC, Excel, SharePoint, PubNub, MQ, BizTalk, GoogleAnalytics, CustomHttpApi, Exchange, Facebook, HDInsight, AzureMarketplace, ActiveDirectory, Hdfs, SharePointDocLib, PowerQueryMashup, OleDb, AdoDotNet, R, LOB, Salesforce, CustomConnector, SAPBWMessageServer, AdobeAnalytics, Essbase, Unknown

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GatewayClusterId
Gateway cluster where the user should be added

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

### -Role
Role to apply to this user on the cluster

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
