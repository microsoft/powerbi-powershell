---
external help file: Microsoft.PowerBI.Commands.Workspaces.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Workspaces
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.workspaces/get-powerbiworkspace?view=powerbi-ps
schema: 2.0.0
---

# Get-PowerBIWorkspace

## SYNOPSIS
Returns a list of Power BI workspaces.

## SYNTAX

### List (Default)
```
Get-PowerBIWorkspace [-Scope <PowerBIUserScope>] [-Filter <String>] [-User <String>] [-Deleted] [-Orphaned]
 [-First <Int32>] [-Skip <Int32>] [-Include <ArtifactType[]>] [<CommonParameters>]
```

### Id
```
Get-PowerBIWorkspace -Id <Guid> [-Scope <PowerBIUserScope>] [-Include <ArtifactType[]>] [<CommonParameters>]
```

### Name
```
Get-PowerBIWorkspace -Name <String> [-Scope <PowerBIUserScope>] [-Include <ArtifactType[]>]
 [<CommonParameters>]
```

### All
```
Get-PowerBIWorkspace [-Scope <PowerBIUserScope>] [-Filter <String>] [-User <String>] [-Deleted] [-Orphaned]
 [-Include <ArtifactType[]>] [-All] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI workspaces that match the specified search criteria and scope.
By default (without -First parameter) it shows the first 100 workspaces assigned to the user. Use -First and -Skip to fetch more workspaces or use -All to return all workspaces.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIWorkspace
```

Returns the first 100 Power BI workspaces the calling user is assigned to (-Scope Individual).

### Example 2
```powershell
PS C:\> Get-PowerBIWorkspace -All
```

Returns all Power BI workspaces the calling user is assigned to.

### Example 3
```powershell
PS C:\> Get-PowerBIWorkspace -Scope Organization -Filter "tolower(name) eq 'contoso sales'"
```

Returns a workspace named 'Contoso Sales' (case insensitive with tolower) within the user's organization.

### Example 3
```powershell
PS C:\> Get-PowerBIWorkspace -Scope Organization -Include All
```

Returns all Power BI workspaces along with related reports, dashboards, datasets, dataflows and workbooks within the user's organization.

## PARAMETERS

### -All
Indicates to show all the workspaces. -First and -Skip cannot be used with this parameter.

```yaml
Type: SwitchParameter
Parameter Sets: All
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Deleted
Indicates to show only deleted workspaces. Only supported when -Scope Organization is specified.

```yaml
Type: SwitchParameter
Parameter Sets: List, All
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Filter
OData filter, case-sensitive (element names start lowercase).

```yaml
Type: String
Parameter Sets: List, All
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -First
First (top) list of results. This value defaults to 100.

```yaml
Type: Int32
Parameter Sets: List
Aliases: Top

Required: False
Position: Named
Default value: 100
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
ID of the workspace to return.

```yaml
Type: Guid
Parameter Sets: Id
Aliases: GroupId, WorkspaceId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Include
Retrieves related artifacts associated with the Power BI workspace. Only available when -Scope is Organization.

```yaml
Type: ArtifactType[]
Parameter Sets: (All)
Aliases: Expand
Accepted values: Reports, Dashboards, Datasets, Dataflows, Workbooks, All

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the workspace to return if one exists with that name. Case insensitive search.

```yaml
Type: String
Parameter Sets: Name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Orphaned
Indicates to show only orphaned workspaces. Only supported when -Scope Organization is specified.

```yaml
Type: SwitchParameter
Parameter Sets: List, All
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. Individual returns only workspaces assigned to the caller; Organization returns all workspaces within a tenant (must be an administrator to initiate). Individual is the default.

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
Parameter Sets: List
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -User
Filter workspaces to show ones which the user is contained within. Only available when -Scope is Organization.

```yaml
Type: String
Parameter Sets: List, All
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Workspaces.Workspace, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
