---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Get-PowerBIWorkspaceEncryptionStatus

## SYNOPSIS
List Power BI workspace encryption status.

## SYNTAX

### Name (Default)
```
Get-PowerBIWorkspaceEncryptionStatus -Name <String> [<CommonParameters>]
```

### Id
```
Get-PowerBIWorkspaceEncryptionStatus -Id <Guid> [<CommonParameters>]
```

### Workspace
```
Get-PowerBIWorkspaceEncryptionStatus -Workspace <Workspace> [<CommonParameters>]
```

## DESCRIPTION
Retrieves the encryption status of datasets for a given workspace.
Operates against all workspaces within a tenant (Organization scope) that is encrypted by customer owned key.

The encryption status enumeration represents the following situation:

* Unknown - Unable to determine state due to dataset corruption.
* NotSupported - Encryption is not supported for this dataset.
* InSyncWithWorkspace - Encryption is supported and is in sync with the encryption settings.
* NotInSyncWithWorkspace - Encryption is supported and not in sync with the encryption settings.

Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.
This cmdlet requires the calling user to be a tenant administrator of the Power BI service.

## EXAMPLES

### Example 1
```
PS C:\> Get-PowerBIWorkspaceEncryptionStatus -Id '101be2ef-c88a-4291-9322-7e9b89ab665f'
```

### Example 2
```
PS C:\> Get-PowerBIWorkspaceEncryptionStatus -Name 'Contoso Sales'
```

### Example 3
```
PS C:\> Get-PowerBIWorkspaceEncryptionStatus -Workspace ( Get-PowerBIWorkspace -Scope Organization -Name "Contoso Sales")
```

### Example 4
```
PS C:\> Get-PowerBIWorkspace -Scope Organization | Get-PowerBIWorkspaceEncryptionStatus
```

## PARAMETERS

### -Id
ID of the workspace to return datasets.

```yaml
Type: Guid
Parameter Sets: Id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the workspace to return datasets.

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

### -Workspace
Workspace from which datasets need to be returned.

```yaml
Type: Workspace
Parameter Sets: Workspace
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Encryption.Dataset, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS

