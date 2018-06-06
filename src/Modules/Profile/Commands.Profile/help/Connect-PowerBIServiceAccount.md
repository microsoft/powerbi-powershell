---
external help file: Microsoft.PowerBI.Commands.Profile.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Profile
online version: https://docs.microsoft.com/en-us/powershell/module/microsoftpowerbimgmt.profile/connect-powerbiserviceaccount?view=powerbi-ps-1.0.193
schema: 2.0.0
---

# Connect-PowerBIServiceAccount

## SYNOPSIS
Login to Power BI service.

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
Login to Power BI service with either a user or service principal account (application key or certificate).
For user accounts, an Azure Active Directory (AAD) First-Party application is leveraged for authentication.
To logout call Disconnect-PowerBIServiceAccount.

## EXAMPLES

### Example 1
```powershell
PS C:\> Connect-PowerBIServiceAccount
```

Logs in using user authentication against the Public cloud, a prompt will display to collect credentials.

### Example 2
```powershell
PS C:\> Connect-PowerBIServiceAccount -Environment China
```

Logs in using user authentication against the China cloud, a prompt will display to collect credentials.

### Example 3
```powershell
PS C:\> Connect-PowerBIServiceAccount -ServicePrincipal -Credential (Get-Credential)
```

Logs in using a service principal against the Public cloud, a prompt will display from Get-Credential to enter your username (your AAD client ID) and password (your application secret key).

### Example 4
```powershell
PS C:\> Connect-PowerBIServiceAccount -ServicePrincipal -CertificateThumbprint 38DA4BED389A014E69A6E6D8AE56761E85F0DFA4 -ApplicationId b5fde143-722c-4e8d-8113-5b33a9291468
```

Logs in using a service principal with an installed certificate to the Public cloud. 
The certificate must be installed in either CurrentUser or LocalMachine certificate store (LocalMachine requires administrator access) with a private key installed.

## PARAMETERS

### -ApplicationId
Azure Active Directory (AAD) application ID (also known as Client ID) to be used with a certificate thumbprint (-CertificateThumbprint) to authenticate with a service principal account (-ServicePrincipal).

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
Certificate thumbprint of an installed certificate associated to an Azure Active Directory (AAD) application.
Certificate must be installed in either the CurrentUser or LocalMachine personal certificate stores (LocalMachine requires an administrator prompt to access) with a private key installed.

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
PSCredential representing the Azure Active Directory (AAD) application client ID (username) and application secret key (password) to authenticate with a service principal account (-ServicePrincipal).

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
Cloud environment to connect against. Default is Public.

```yaml
Type: PowerBIEnvironmentType
Parameter Sets: (All)
Aliases:
Accepted values: Public, Germany, USGov, China, OneBox, EDog, DXT

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ServicePrincipal
Indicates to use a service principal account, as opposed to a user account.

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
