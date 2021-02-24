---
external help file: Microsoft.PowerBI.Commands.Profile.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Profile
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.profile/resolve-powerbierror?view=powerbi-ps
schema: 2.0.0
---

# Resolve-PowerBIError

## SYNOPSIS
Shows detailed error information produced from MicrosoftPowerBIMgmt.* cmdlets.

## SYNTAX

### AnyError (Default)
```
Resolve-PowerBIError [-Error <ErrorRecord[]>] [<CommonParameters>]
```

### LastError
```
Resolve-PowerBIError [-Last] [<CommonParameters>]
```

## DESCRIPTION
Outputs additional information for any errors produced from MicrosoftPowerBIMgmt.* cmdlets.

## EXAMPLES

### Example 1
```powershell
PS C:\> Resolve-PowerBIError
```

Displays all errors occurring in the PowerShell session.

### Example 2
```powershell
PS C:\> Resolve-PowerBIError -Last
```

Displays the last error occurring in the PowerShell session.

## PARAMETERS

### -Error
List of errors to display. For example, $Error[0..2] will show the last three errors.

```yaml
Type: ErrorRecord[]
Parameter Sets: AnyError
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Last
Indicates to show the last error occurring in the PowerShell session.

```yaml
Type: SwitchParameter
Parameter Sets: LastError
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Management.Automation.ErrorRecord[]

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

