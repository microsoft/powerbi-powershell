---
external help file: Microsoft.PowerBI.Commands.Reports.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Reports
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.reports/get-powerbidashboard?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIDashboard

## SYNOPSIS
Returns a list of Power BI dashboards.

## SYNTAX

### List (Default)
```
Get-PowerBIDashboard [-Scope <PowerBIUserScope>] [-Filter <String>] [-First <Int32>] [-Skip <Int32>]
 [-WorkspaceId <Guid>] [<CommonParameters>]
```

### Id
```
Get-PowerBIDashboard -Id <Guid> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### ObjectAndId
```
Get-PowerBIDashboard -Id <Guid> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### Name
```
Get-PowerBIDashboard -Name <String> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### ObjectAndName
```
Get-PowerBIDashboard -Name <String> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### ObjectAndList
```
Get-PowerBIDashboard [-Scope <PowerBIUserScope>] [-Filter <String>] [-First <Int32>] [-Skip <Int32>]
 -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI dashboards that match the specified search criteria and scope.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDashboard
```

Returns all Power BI dashboards that the calling user is assigned to (-Scope Individual).

### Example 2
```powershell
PS C:\> Get-PowerBIDashboard -Scope Organization -Name 'Contoso Sales'
```

Returns a workspace named 'Contoso Sales' (case insensitive) within the user's organization.

## PARAMETERS

### -Filter
OData filter, case-sensitive (element names start lowercase). Only supported when -Scope Organization is specified.

```yaml
Type: String
Parameter Sets: List, ObjectAndList
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -First
First (top) list of results.

```yaml
Type: Int32
Parameter Sets: List, ObjectAndList
Aliases: Top

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Id of the dashboard to return.

```yaml
Type: Guid
Parameter Sets: Id, ObjectAndId
Aliases: ImportId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the dashboard to return if one exists with that name. Case insensitive search.

```yaml
Type: String
Parameter Sets: Name, ObjectAndName
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates the scope of the call. Individual returns only dashboards assigned to the caller; Organization returns all dashboards within a tenant (must be an administrator to initiate). Individual is the default.

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

### -Skip
Skips the first set of results.

```yaml
Type: Int32
Parameter Sets: List, ObjectAndList
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace to filter results to; only dashboards that belong to that workspace are shown.

```yaml
Type: Workspace
Parameter Sets: ObjectAndId, ObjectAndName, ObjectAndList
Aliases: Group

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WorkspaceId
Workspace Id to filter results to; only dashboards that belong to that workspace are shown.

```yaml
Type: Guid
Parameter Sets: List, Id, Name
Aliases: GroupId

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

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Reports.Dashboard, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
