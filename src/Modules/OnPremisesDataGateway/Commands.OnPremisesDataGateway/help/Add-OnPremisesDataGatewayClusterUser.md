---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Add-OnPremisesDataGatewayClusterUser

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

```
Add-OnPremisesDataGatewayClusterUser [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -PrincipalObjectId <Guid>
 [-AllowedDataSourceTypes <System.Collections.Generic.IEnumerable`1[Microsoft.PowerBI.Common.Api.Gateways.Entities.DatasourceType]>]
 -Role <String> [<CommonParameters>]
```

## DESCRIPTION
{{Fill in the Description}}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -AllowedDataSourceTypes
{{Fill AllowedDataSourceTypes Description}}

```yaml
Type: System.Collections.Generic.IEnumerable`1[Microsoft.PowerBI.Common.Api.Gateways.Entities.DatasourceType]
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
{{Fill GatewayClusterId Description}}

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

### -PrincipalObjectId
{{Fill PrincipalObjectId Description}}

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
{{Fill Role Description}}

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
