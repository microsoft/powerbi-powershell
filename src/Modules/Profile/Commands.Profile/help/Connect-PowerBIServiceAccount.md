---
external help file: Microsoft.PowerBI.Commands.Profile.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Profile
online version:
schema: 2.0.0
---

# Connect-PowerBIServiceAccount

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### User (Default)
```
Connect-PowerBIServiceAccount [-Environment <PowerBIEnvironmentType>] [-Credential <PSCredential>]
 [<CommonParameters>]
```

### ServicePrincipal
```
Connect-PowerBIServiceAccount [-Environment <PowerBIEnvironmentType>] -Credential <PSCredential>
 [-ServicePrincipal] [<CommonParameters>]
```

### ServicePrincipalCertificate
```
Connect-PowerBIServiceAccount [-Environment <PowerBIEnvironmentType>] -CertificateThumbprint <String>
 -ApplicationId <String> [-ServicePrincipal] [<CommonParameters>]
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

### -ApplicationId
{{Fill ApplicationId Description}}

```yaml
Type: String
Parameter Sets: ServicePrincipalCertificate
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CertificateThumbprint
{{Fill CertificateThumbprint Description}}

```yaml
Type: String
Parameter Sets: ServicePrincipalCertificate
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Credential
{{Fill Credential Description}}

```yaml
Type: PSCredential
Parameter Sets: User
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: PSCredential
Parameter Sets: ServicePrincipal
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Environment
{{Fill Environment Description}}

```yaml
Type: PowerBIEnvironmentType
Parameter Sets: (All)
Aliases:
Accepted values: Public, PPE

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ServicePrincipal
{{Fill ServicePrincipal Description}}

```yaml
Type: SwitchParameter
Parameter Sets: ServicePrincipal, ServicePrincipalCertificate
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

### Microsoft.PowerBI.Common.Abstractions.Interfaces.IPowerBIProfile

## NOTES

## RELATED LINKS
