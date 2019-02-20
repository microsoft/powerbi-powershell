---
external help file: Microsoft.PowerBI.Commands.Admin.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Admin
online version:
schema: 2.0.0
---

# Get-PowerBIWorkspaceEncryptionStatus

## SYNOPSIS
List workspace encryption status

## SYNTAX

```
Get-PowerBIWorkspaceEncryptionStatus -Name <String> [<CommonParameters>]
```

## DESCRIPTION
Retrives the encryotion status of dataset for a given workspace name

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PowerBIWorkspaceEncryptionStatus -Name 'WorkspaceName'
```

## PARAMETERS

### -Name
The name of the workspace

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
