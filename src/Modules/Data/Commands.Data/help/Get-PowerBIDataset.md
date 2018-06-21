---
external help file: Microsoft.PowerBI.Commands.Data.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Data
online version:
schema: 2.0.0
---

# Get-PowerBIDataset

## SYNOPSIS
Returns a list of Power BI datasets.

## SYNTAX

### List (Default)
```
Get-PowerBIDataset [-Scope <PowerBIUserScope>] [-Filter <String>] [-First <Int32>] [-Skip <Int32>]
 [-WorkspaceId <Guid>] [<CommonParameters>]
```

### Id
```
Get-PowerBIDataset -Id <Guid> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### ObjectAndId
```
Get-PowerBIDataset -Id <Guid> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### Name
```
Get-PowerBIDataset -Name <String> [-Scope <PowerBIUserScope>] [-WorkspaceId <Guid>] [<CommonParameters>]
```

### ObjectAndName
```
Get-PowerBIDataset -Name <String> [-Scope <PowerBIUserScope>] -Workspace <Workspace> [<CommonParameters>]
```

### ObjectAndList
```
Get-PowerBIDataset [-Scope <PowerBIUserScope>] [-First <Int32>] [-Skip <Int32>] -Workspace <Workspace>
 [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI datasets with the provided search criteria and scope specified.
You must have logged in previously before, using Connect-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIDataset
```

Returns a list of all Power BI datasets a user has access to.

### Example 2
```powershell
PS C:\> Get-PowerBIDataset -Scope Organization
```

Returns a list of all Power BI datasets within a user's organization.

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
Id of the dataset to return.

```yaml
Type: Guid
Parameter Sets: Id, ObjectAndId
Aliases: DatasetId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the dataset to return if one exists with that name. Case insensitive search.

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
Indicates scope of the call. Individual returns only datasets assigned to the caller; Organization returns all datasets within a tenant (must be an administrator to initiate). Individual is the default.

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
Workspace to filter results to, datasets only belonging to that workspace are shown.

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
Workspace ID to filter results to, datasets only belonging to that workspace are shown.

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
