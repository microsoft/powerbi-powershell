---
external help file: Microsoft.PowerBI.Commands.Capacities.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Capacities
online version:
schema: 2.0.0
---

# Get-PowerBICapacity

## SYNOPSIS
Returns a list of Power BI capacities.

## SYNTAX

```
Get-PowerBICapacity [-Scope <PowerBIUserScope>] [-Show <PowerBIGetCapacityExpandEnum>] [<CommonParameters>]
```

## DESCRIPTION
Retrieves a list of Power BI capacities that matches the specified scope.

Before you run this command, make sure you log in using Connect-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```
PS C:\> Get-PowerBICapacity -Scope Organization -Show EncryptionKey
```

## PARAMETERS

### -Scope
Indicates scope of the call. Individual returns only capacities assigned to the caller; Organization returns all capacities within a tenant (must be an administrator to initiate). Individual is the default.

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

### -Show
Expands related entities inline

```yaml
Type: PowerBIGetCapacityExpandEnum
Parameter Sets: (All)
Aliases:
Accepted values: None, EncryptionKey

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

### System.Collections.Generic.IEnumerable`1[[Microsoft.PowerBI.Common.Api.Capacities.Capacity, Microsoft.PowerBI.Common.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]

## NOTES

## RELATED LINKS
