---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.data/get-powerbidataflow?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIDataflow

## SYNOPSIS
Returns a list of Power BI dataflows.

## SYNTAX

### List (Default)
```
Get-PowerBIDataflow [-Scope <PowerBIUserScope>] [-Filter <String>] [-First <Int32>] [-Skip <Int32>]
 [-WorkspaceId <Guid>] [<CommonParameters>]
```

### Id
```
Get-PowerBIDataflow -Id <Guid> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### WorkspaceAndId
```
Get-PowerBIDataflow -Id <Guid> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### Name
```
Get-PowerBIDataflow -Name <String> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### WorkspaceAndName
```
Get-PowerBIDataflow -Name <String> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### WorkspaceAndList
```
Get-PowerBIDataflow [-Scope <PowerBIUserScope>] [-Filter <String>] [-First <Int32>] [-Skip <Int32>]
 -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI dataflows that match the specified search criteria and scope.
For -Scope Individual, dataflows are returned only from the specified workspace, using the given -Workspace or -WorkspaceId parameters.
For -Scope Organization, dataflows could be returned from the entire user's organization.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDataflow -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375
```

Returns a list of all Power BI dataflows in workpsace with ID ed451706-2b02-430c-b25c-20c0842c6375 that the user has access to.

### Example 2
```powershell
PS C:\> Get-PowerBIDataflow -Scope Organization
```

Returns a list of all Power BI dataflows within a user's organization.

### Example 3
```powershell
PS C:\> Get-PowerBIDataflow -Name "MyDataflow" -Scope Organization
```

Returns a dataflow with the Name "MyDataflow" from within all the organization.

### Example 4
```powershell
PS C:\> Get-PowerBIDataflow -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375 -First 20
```

Returns a list of the first 20 dataflows in a workspace with ID ed451706-2b02-430c-b25c-20c0842c6375 that the user has access to.

### Example 5
```powershell
PS C:\> Get-PowerBIDataflow -WorkspaceId ed451706-2b02-430c-b25c-20c0842c6375 -Id 672403a7-34b7-493c-8ab1-3f1066573dc5
```

Returns a dataflow with ID 672403a7-34b7-493c-8ab1-3f1066573dc5 in a workspace with ID ed451706-2b02-430c-b25c-20c0842c6375.

### Example 6
```powershell
PS C:\> Get-PowerBIDataflow -Scope Organization -Filter "configuredBy eq 'john@contoso.com'"
```

Returns all dataflows configured by 'john@contoso.com' within the user's organization.

## PARAMETERS

### -Filter
OData filter, case-sensitive (element names start lowercase). Only supported when -Scope Organization is specified.

```yaml
Type: String
Parameter Sets: List, WorkspaceAndList
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
Parameter Sets: List, WorkspaceAndList
Aliases: Top

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the dataflow to return.

```yaml
Type: Guid
Parameter Sets: Id, WorkspaceAndId
Aliases: DataflowId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the dataflow to return if one exists with that name. Case insensitive search.

```yaml
Type: String
Parameter Sets: Name, WorkspaceAndName
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates the scope of the call. Individual scope must be run with -Workspace or -WorkspaceId parameters. Organization scope returns all dataflows within a tenant (must be an administrator to initiate). Individual is the default.

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

### -Skip
Skips the first set of results.

```yaml
Type: Int32
Parameter Sets: List, WorkspaceAndList
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Workspace
Workspace to filter results to, only dataflows belonging to that workspace are shown.
Supports pipelining of Cmdlets that return Workspace objects.

```yaml
Type: Workspace
Parameter Sets: WorkspaceAndId, WorkspaceAndName, WorkspaceAndList
Aliases: Group

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -WorkspaceId
ID of the workspace to filter results to, only dataflows belonging to that workspace are shown.

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

### System.Object

## NOTES

## RELATED LINKS
