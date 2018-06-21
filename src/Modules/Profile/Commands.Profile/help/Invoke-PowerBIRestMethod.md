---
external help file: Microsoft.PowerBI.Commands.Profile.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.Profile
online version:
schema: 2.0.0
---

# Invoke-PowerBIRestMethod

## SYNOPSIS
Executes a REST call to the Power BI service, with the specified URL and body.

## SYNTAX

```
Invoke-PowerBIRestMethod -Url <String> -Method <PowerBIWebRequestMethod> [-Body <String>] [-OutFile <String>]
 [-Organization <String>] [-Version <String>] [<CommonParameters>]
```

## DESCRIPTION
Invokes a REST request against the Power BI service using the profile you're logged in with.
The REST verb can be specified using the -Method parameter. The -Body parameter is required for the verbs POST, PUT, and PATCH.
Before you run this command, make sure you log in using Connect-PowerBIServiceAccount. 

## EXAMPLES

### Example 1
```powershell
PS C:\> Invoke-PowerBIRestMethod -Url 'groups' -Method Get
```

Invokes the URL https://api.powerbi.com/v1.0/myorg/groups with the GET method\verb.

## PARAMETERS

### -Body
Body of the request, also known as *content*. This is optional unless the request method is POST, PUT, or PATCH.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Method
Type of HTTP request method\verb to make with the call.

```yaml
Type: PowerBIWebRequestMethod
Parameter Sets: (All)
Aliases:
Accepted values: Get, Post, Delete, Put, Patch, Options

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Organization
Organization name or tenant GUID to include in the URL. Default is 'myorg'.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutFile
Output file for writing the response content to. File path specified must not exist.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Url
Relative or absolute URL of the Power BI entity you want to access. For example, if you want to access https://api.powerbi.com/v1.0/myorg/groups, then specify 'groups', or pass in the entire URL.

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

### -Version
Version of the API to include in the URL. Default is 'v1.0'. Ignored if -Url is an absolute URL.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

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
