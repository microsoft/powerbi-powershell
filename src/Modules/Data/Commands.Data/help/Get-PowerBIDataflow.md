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

### ObjectAndId
```
Get-PowerBIDataflow -Id <Guid> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### Name
```
Get-PowerBIDataflow -Name <String> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### ObjectAndName
```
Get-PowerBIDataflow -Name <String> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### ObjectAndList
```
Get-PowerBIDataflow [-Scope <PowerBIUserScope>] [-First <Int32>] [-Skip <Int32>] -Workspace <Workspace>
 [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI dataflows that match the specified search criteria and scope.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 
For -Scope Individual, dataflows are returned from only from the specified Workspace, using the given -Workspace or -WorkspaceId parameter.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDataflow
```

Returns a list of all Power BI dataflows a user has access to.

### Example 2
```powershell
PS C:\> Get-PowerBIDataflow -Scope Organization
```

Returns a list of all Power BI dataflows within a user's organization.

## PARAMETERS

### -Filter
OData filter, case-sensitive (element names start lowercase). Only supported when -Scope Organization is specified.

```yaml
Type: String
Parameter Sets: List
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
ID of the dataflow to return.

```yaml
Type: Guid
Parameter Sets: Id, ObjectAndId
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
Parameter Sets: Name, ObjectAndName
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
Indicates scope of the call. With -Workspace or -WorkspaceId, dataflows under the workspace assigned to the caller are returned; Individual will work only with -Workspace or -WorkspaceId parameters. Organization returns all dataflows within a tenant (must be an administrator to initiate). Individual is the default.

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
Workspace to filter results to, dataflows only belonging to that workspace are shown.

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
ID of the workspace to filter results to, dataflows only belonging to that workspace are shown.

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
