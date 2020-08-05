#region Log Utilities
# ==================================================================
# Verbose flag
# ==================================================================
[Boolean]$glovalVerbose=$false

# ==================================================================
# Function to set verbose flag
# ==================================================================
function SetVerbose([Boolean]$v)
{
	$global:glovalVerbose = $v
}

# ==================================================================
# Console logging method
# ==================================================================
function SafeLog
{
	[CmdletBinding()]
	Param( 
		[Parameter(Mandatory=$true, Position = 0)][String]$message,
		[Parameter(Mandatory=$false)][String]$color
	)
	try
	{
		$ErrorActionPreference="SilentlyContinue"
		if ($color -eq $null) 
		{
			$color = "Green"
		}
	   	$messageInternal = "$(get-date -format `"hh:mm:ss`"):" + $message
	   	write-host $messageInternal -foregroundcolor $color
		$ErrorActionPreference = "Continue"
	}
	catch
	{
	}
}

# ==================================================================
# Log and throw error
# ==================================================================
function DFThrowError($message)
{
	SafeLog -message $message -color "Red"
	throw $message
}

# ==================================================================
# Log error
# ==================================================================
function DFLogError($message)
{
    SafeLog -message $message -color "Red"
}

# ==================================================================
# Log verbose
# ==================================================================
function DFLogVerbose($message)
{
	if ($global:glovalVerbose)
	{
		SafeLog -message $message -color "Gray"
	}
}

# ==================================================================
# Log message
# ==================================================================
function DFLogMessage($message)
{
    SafeLog -message $message -color "Blue"
}

# ==================================================================
# Log warning
# ==================================================================
function DFLogWarning($message)
{
    SafeLog -message $message -color "Yellow"
}
#endregion

#region Power BI Service Utilities

# ==================================================================
# Logs into a Power BI environment
# ==================================================================
function LoginPowerBi([String]$Environment)
{
	DFLogMessage("Logging in to PowerBI")
	if ($Environment -ne "" -and $Environment -ne $null)
    {
        Connect-PowerBIServiceAccount -Environment $Environment
    }
    else 
    {
        Login-PowerBI
    }
}

# ==================================================================
# Verifies and gets a workspace id from Power BI
# ==================================================================
function GetWorkspaceIdFromName([String]$workspaceName)
{
	DFLogMessage("Getting workspace info : $workspaceName")
	$myRestResult = Invoke-PowerBIRestMethod -Url 'Groups' -Method Get | ConvertFrom-Json

    DFLogVerbose("Looking for workspace $workspaceName")
	foreach ($item in $myRestResult.value) 
	{
		if ($item.name -eq $workspaceName) 
		{
			$id = $item.id
            DFLogMessage("Workspace Name:$workspaceName Id:$id")
			return $item.id
        }
	}
	
	DFThrowError("Workspace [$workspaceName] not found.")	
}

# ==================================================================
# Gets a list of dataflows in a workspace
# ==================================================================
function GetDataflowsForWorkspace([String]$workspaceId)
{
	DFLogMessage("Getting list of dataflows from workspace Id:$workspaceId")
	$myRestResult = Invoke-PowerBIRestMethod -Url "groups/$workspaceId/dataflows" -Method Get | ConvertFrom-Json
	[Hashtable]$dataflows = @{}

	foreach ($item in $myRestResult.value) 
	{
		$id =$item.objectId
		$name =$item.name
		DFLogVerbose("Dataflow Id:$id Name:$name")
		$dataflows[$id] = $item
	}

	DFLogMessage("Fetched dataflows. Count: " + $dataflows.Count)
    return $dataflows;
}

# ==================================================================
# Gets the content of a dataflow
# ==================================================================
function GetDataflow([String]$workspaceId, [String]$dataflowId, [String]$dataflowName)
{
	DFLogMessage("Downloading dataflow Id:$dataflowId Name:$dataflowName")
	$modelJson = Invoke-PowerBIRestMethod -Url /groups/$workspaceId/dataflows/$dataflowId -Method Get | ConvertFrom-Json
	return $modelJson
}

# ==================================================================
# Gets the list of reference models by parsing a model.json file
# ==================================================================
function GetReferenceModels($modelJson)
{
	$referenceModels= @()
	foreach ($item in $modelJson.referenceModels) 
	{
		$parts = $item.id -split "/"
		$s = '{"WorkspaceId":"' + $parts[0] + '","DataflowId":"' + $parts[1] + '", "Location":"' + $item.location + '"}'
		$referenceModel =  $s | ConvertFrom-Json
		DFLogVerbose("Reference model: $referenceModel")
		$referenceModels += $referenceModel
	}

	return $referenceModels
}

# ==================================================================
# Removes partitions from a model.json
# ==================================================================
function PrepareForImport($modelJson)
{
	DFLogVerbose("Removing partitions from model")
	for ($i=0; $i -lt $modelJson.entities.Count; $i++)
	{
		if(Get-Member -inputobject $modelJson.entities[$i] -name "partitions" -Membertype Properties)
		{
            $modelJson.entities[$i].PSObject.properties.remove('partitions')
		}
	}
}

# ==================================================================
# Fixes the reference links in a model.json
# ==================================================================
function FixReference($modelJson, $lookupModelId, $workspaceId, $modelId)
{
	$newReference = "$workspaceId/$modelId"
	DFLogVerbose("Looking for references: $lookupModelId to $newReference")
	for ($i=0; $i -lt $modelJson.referenceModels.Count; $i++)
	{
		DFLogVerbose("Reference model: " + $modelJson.referenceModels[$i].Id)
		if ($modelJson.referenceModels[$i].Id.contains($lookupModelId))
		{
			$oldReference = $modelJson.referenceModels[$i].Id
			DFLogVerbose("Found references: $oldReference")
			$modelJson.referenceModels[$i].Id = $newReference
			$modelJson.referenceModels[$i].Location = $null

			for ($j=0; $j -lt $modelJson.entities.Count; $j++)
			{
				if ($modelJson.entities[$j].modelId -eq $oldReference)
				{
					$modelJson.entities[$j].modelId = $newReference
				}
			}
		}
	}
}

# ==================================================================
# Imports a model.json and returns its id once imported
# ==================================================================
function ImportModel($workspaceId, $modelId, $modelJson, $dataflows)
{
	PrepareForImport($modelJson)

	# Generate the payload
	$string_json = $modelJson | ConvertTo-Json -depth 100
    $boundary = [System.Guid]::NewGuid().ToString(); 
    $LF = "`r`n";
    $bodyLines = ( 
        "--$boundary",
        "Content-Disposition: form-data; name=`"file`"; filename=`"$FieldName`"",
        "Content-Type: application/json$LF",
        $string_json,
        "--$boundary--$LF" 
	) -join $LF
	
	# Call method to start import
	$importUri = "https://api.powerbi.com/v1.0/myorg/groups/$workspaceId/imports?datasetDisplayName=model.json";
	$overwriteMode = $false
	if ($null -ne $modelId)
	{
		$importUri += "&nameConflict=Overwrite"
		$overwriteMode = $true
	}
	else 
	{
		$importUri += "&nameConflict=GenerateUniqueName"
	}

	$startDate = (Get-Date).ToUniversalTime()
	$token = Get-PowerBIAccessToken -AsString
	$headers = @{Authorization = "$token"}
    $response = $null
	try 
	{
		$response = Invoke-RestMethod -Uri $importUri -Method Post -ContentType "multipart/form-data; boundary=`"$boundary`"" -Headers $headers -Body ([System.Text.Encoding]::UTF8.GetBytes($bodyLines))
		DFLogMessage("Started model import Id: " + $response.id + " at UTC: $startDate")
	}
	catch 
	{
		$x = $_ | ConvertFrom-Json
		$code = $x.error.code
		$message = $x.error.message
		DFThrowError("Import failed. Code=$code, Message=$message")
	}

	# Wait for import to be completed
	Start-Sleep -Seconds 1.0
    $importId = $response.id
	$importState = 'Publishing'
	$lastUpdated = $startDate
	$importStatus = $null
	$counter = 0
	$maxIterations = 3 * 60
	while($importState -eq 'Publishing' -or $lastUpdated -le $startDate)
	{
		try
		{
			$importStatus = Invoke-PowerBIRestMethod -Url /groups/$workspaceId/imports/$importId -Method Get | ConvertFrom-Json
			$importState = $importStatus.importState
			$lastUpdated = ([datetime]::Parse($importStatus.updatedDateTime)).ToUniversalTime() 
			DFLogVerbose("Started=$startDate importState=$importState LastUpdated=$lastUpdated")
			Start-Sleep -Seconds 1.0
			$counter += 1

			if ($overwriteMode -and $counter -ge $maxIterations)
			{
				break
			}
		} 
		catch 
		{
            $x = $_ | ConvertFrom-Json
			$code = $x.error.code
			$message = $x.error.message
			DFThrowError("Import failed. Code=$code, Message=$message")
        }
	}
	
	# Check for errors
	if ($importState.equals('Failed'))
	{
        $code = $importStatus.error.code
        $details = $importStatus.error.details
        DFThrowError("Import Failed with code [$code] and details [$details]")
	}
	if ($overwriteMode -and $counter -ge $maxIterations)
	{
		DFThrowError("Import failed. Request timed out")
	}
	
	# Obtain the new model id
	if ($null -ne $modelId)
	{
		return $dataflows[$modelId]
	}
	else 
	{
		$newDataflows = GetDataflowsForWorkspace($workspaceId)

		foreach ($dataflowid in $newDataflows.Keys) 
		{
			if ($null -eq $dataflows[$dataflowid])
			{
				DFLogVerbose("Found imported dataflow=$dataflowid")
				return $newDataflows[$dataflowid]
			}
		}
	}

	DFThrowError("Unexpected error. Cannot find imported model in the workspace")
}

# ==================================================================
# Reads a model.json from a file
# ==================================================================
function ReadModelJson([String]$fileName)
{
	If(!(test-path $fileName))
    {
        DFThrowError("File $fileName does not exist")
    }
	$json = Get-Content -Encoding UTF8 -Raw -Path $fileName | ConvertFrom-Json
	return $json
}

# ==================================================================
# Finds a model id to overwrite
# ==================================================================
function GetOverrwiteModelId($dataflows, $overwrite, $modelName)
{
	$overwriteModelId = $null
	if ($overwrite)
	{
		foreach ($dataflow in $dataflows.Values) 
		{
			if ($modelName -eq $dataflow.name)
			{
				$overwriteModelId = $dataflow.objectId
			}
		}
	}

	DFLogVerbose("Overwrite for model $modelName : $overwriteModelId")
	return $overwriteModelId
}


#endregion

#region File Utilities

# ==================================================================
# Verifies if a folder exists
# ==================================================================
function VerifyDirectory([String]$directoryName)
{
	If(!(test-path $directoryName))
    {
        DFThrowError("Directory $directoryName does not exist")
    }
}

# ==================================================================
# Creates a folder if it does not exist
# ==================================================================
function CreateDirectoryIfNotExists([String]$directoryName)
{
	If(!(test-path $directoryName))
    {
        DFLogMessage("Creating folder $directoryName")
        New-Item -ItemType Directory -Force -Path $directoryName
    }
}

# ==================================================================
# Deletes a file if it exists
# ==================================================================
function DeleteFileIfExists([String]$fileName)
{
	If((test-path $fileName))
    {
        DFLogVerbose("Deleting file $fileName")
        Remove-Item -Path $fileName
    }
}
#endregion
# SIG # Begin signature block
# MIInLAYJKoZIhvcNAQcCoIInHTCCJxkCAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCDoFBCh3TA4gxXm
# G66v88A9INfcXzzVEaCe2e63FL6HeKCCEWUwggh3MIIHX6ADAgECAhM2AAABCXeq
# 7JKeP287AAEAAAEJMA0GCSqGSIb3DQEBCwUAMEExEzARBgoJkiaJk/IsZAEZFgNH
# QkwxEzARBgoJkiaJk/IsZAEZFgNBTUUxFTATBgNVBAMTDEFNRSBDUyBDQSAwMTAe
# Fw0yMDAyMDkxMzIzMzFaFw0yMTAyMDgxMzIzMzFaMCQxIjAgBgNVBAMTGU1pY3Jv
# c29mdCBBenVyZSBDb2RlIFNpZ24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEK
# AoIBAQCYbOXmlszFnuCPBMUV6BC1J1QOI7+cqKNtzF8TpVghtmKqz3XY46b/YxOH
# xTQp9J6ewnLLEzC2tP5AAx+Txqr1HpNy63D2U8WbBuRpXBk0bb8f8xrJoK7IVtSJ
# i0dNXCQ0E/XDCw6EARlDrZPfRYR26xk1eCHoX6dWBSKzkrQuOAkcTomU0diFVKW1
# O5A77bXGyF+l31eB5GyLjaPd52G6GzJBIYYKAtrfWRdH0Lei/LPqTcm1z3gLML0B
# GKRNvxi4M21jZBE9LwCvkIKLIZiL/PM6IPXdCVhevxTfjfZuB/GEp0SWmL+EdRoe
# ex084siN8ItdLPdg6hvpqXse/idLAgMBAAGjggWDMIIFfzApBgkrBgEEAYI3FQoE
# HDAaMAwGCisGAQQBgjdbAQEwCgYIKwYBBQUHAwMwPQYJKwYBBAGCNxUHBDAwLgYm
# KwYBBAGCNxUIhpDjDYTVtHiE8Ys+hZvdFs6dEoFgg93NZoaUjDICAWQCAQwwggJ2
# BggrBgEFBQcBAQSCAmgwggJkMGIGCCsGAQUFBzAChlZodHRwOi8vY3JsLm1pY3Jv
# c29mdC5jb20vcGtpaW5mcmEvQ2VydHMvQlkyUEtJQ1NDQTAxLkFNRS5HQkxfQU1F
# JTIwQ1MlMjBDQSUyMDAxKDEpLmNydDBSBggrBgEFBQcwAoZGaHR0cDovL2NybDEu
# YW1lLmdibC9haWEvQlkyUEtJQ1NDQTAxLkFNRS5HQkxfQU1FJTIwQ1MlMjBDQSUy
# MDAxKDEpLmNydDBSBggrBgEFBQcwAoZGaHR0cDovL2NybDIuYW1lLmdibC9haWEv
# QlkyUEtJQ1NDQTAxLkFNRS5HQkxfQU1FJTIwQ1MlMjBDQSUyMDAxKDEpLmNydDBS
# BggrBgEFBQcwAoZGaHR0cDovL2NybDMuYW1lLmdibC9haWEvQlkyUEtJQ1NDQTAx
# LkFNRS5HQkxfQU1FJTIwQ1MlMjBDQSUyMDAxKDEpLmNydDBSBggrBgEFBQcwAoZG
# aHR0cDovL2NybDQuYW1lLmdibC9haWEvQlkyUEtJQ1NDQTAxLkFNRS5HQkxfQU1F
# JTIwQ1MlMjBDQSUyMDAxKDEpLmNydDCBrQYIKwYBBQUHMAKGgaBsZGFwOi8vL0NO
# PUFNRSUyMENTJTIwQ0ElMjAwMSxDTj1BSUEsQ049UHVibGljJTIwS2V5JTIwU2Vy
# dmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1BTUUsREM9R0JM
# P2NBQ2VydGlmaWNhdGU/YmFzZT9vYmplY3RDbGFzcz1jZXJ0aWZpY2F0aW9uQXV0
# aG9yaXR5MB0GA1UdDgQWBBSFf5cqMUbeKYe6lzBbR/KdqpCuUjAOBgNVHQ8BAf8E
# BAMCB4AwUAYDVR0RBEkwR6RFMEMxKTAnBgNVBAsTIE1pY3Jvc29mdCBPcGVyYXRp
# b25zIFB1ZXJ0byBSaWNvMRYwFAYDVQQFEw0yMzYxNjcrNDU3Nzg5MIIB1AYDVR0f
# BIIByzCCAccwggHDoIIBv6CCAbuGPGh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9w
# a2lpbmZyYS9DUkwvQU1FJTIwQ1MlMjBDQSUyMDAxLmNybIYuaHR0cDovL2NybDEu
# YW1lLmdibC9jcmwvQU1FJTIwQ1MlMjBDQSUyMDAxLmNybIYuaHR0cDovL2NybDIu
# YW1lLmdibC9jcmwvQU1FJTIwQ1MlMjBDQSUyMDAxLmNybIYuaHR0cDovL2NybDMu
# YW1lLmdibC9jcmwvQU1FJTIwQ1MlMjBDQSUyMDAxLmNybIYuaHR0cDovL2NybDQu
# YW1lLmdibC9jcmwvQU1FJTIwQ1MlMjBDQSUyMDAxLmNybIaBumxkYXA6Ly8vQ049
# QU1FJTIwQ1MlMjBDQSUyMDAxLENOPUJZMlBLSUNTQ0EwMSxDTj1DRFAsQ049UHVi
# bGljJTIwS2V5JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlv
# bixEQz1BTUUsREM9R0JMP2NlcnRpZmljYXRlUmV2b2NhdGlvbkxpc3Q/YmFzZT9v
# YmplY3RDbGFzcz1jUkxEaXN0cmlidXRpb25Qb2ludDAfBgNVHSMEGDAWgBQbZqIZ
# /JvrpdqEjxiY6RCkw3uSvTAfBgNVHSUEGDAWBgorBgEEAYI3WwEBBggrBgEFBQcD
# AzANBgkqhkiG9w0BAQsFAAOCAQEAQYd76y+ByMUA3+A7WsHFVJZpuEyh9fqDOhQ3
# f18LCatNSqBNUP0PYlEVimWJUvrr3RAHWBG2nr3inRaaivvveR51LkM1TH098qVj
# v+7MNcwu8IkQ2d0+OoAfQXSsnFMNXsJBZsT6W3zscdK6YCFmyrPkYOI0NTPhoX+i
# ZvhwtmR9x9UI3dDrd/Lg+9L+H5Cn4UI00llmM89XBpich2vzQR9N+3J98TJn5Yxf
# IXoDYhX6zHu+cKilOjs2uwg3wCL3VekflxyOeyFC7hFTRFWAdWKJ+QM78WCFOElB
# 38ah1U43wk7u+BruSXE/gXyGUi5NIfUsPmEE/S8l9UewoIPcIzCCCOYwggbOoAMC
# AQICEx8AAAAUtMUfxvKAvnEAAAAAABQwDQYJKoZIhvcNAQELBQAwPDETMBEGCgmS
# JomT8ixkARkWA0dCTDETMBEGCgmSJomT8ixkARkWA0FNRTEQMA4GA1UEAxMHYW1l
# cm9vdDAeFw0xNjA5MTUyMTMzMDNaFw0yMTA5MTUyMTQzMDNaMEExEzARBgoJkiaJ
# k/IsZAEZFgNHQkwxEzARBgoJkiaJk/IsZAEZFgNBTUUxFTATBgNVBAMTDEFNRSBD
# UyBDQSAwMTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANVXgQLW+frQ
# 9xuAud03zSTcZmH84YlyrSkM0hsbmr+utG00tVRHgw40pxYbJp5W+hpDwnmJgicF
# oGRrPt6FifMmnd//1aD/fW1xvGs80yZk9jxTNcisVF1CYIuyPctwuJZfwE3wcGxh
# kVw/tj3ZHZVacSls3jRD1cGwrcVo1IR6+hHMvUejtt4/tv0UmUoH82HLQ8w1oTX9
# D7xj35Zt9T0pOPqM3Gt9+/zs7tPp2gyoOYv8xR4X0iWZKuXTzxugvMA63YsB4ehu
# SBqzHdkF55rxH47aT6hPhvDHlm7M2lsZcRI0CUAujwcJ/vELeFapXNGpt2d3wcPJ
# M0bpzrPDJ/8CAwEAAaOCBNowggTWMBAGCSsGAQQBgjcVAQQDAgEBMCMGCSsGAQQB
# gjcVAgQWBBSR/DPOQp72k+bifVTXCBi7uNdxZTAdBgNVHQ4EFgQUG2aiGfyb66Xa
# hI8YmOkQpMN7kr0wggEEBgNVHSUEgfwwgfkGBysGAQUCAwUGCCsGAQUFBwMBBggr
# BgEFBQcDAgYKKwYBBAGCNxQCAQYJKwYBBAGCNxUGBgorBgEEAYI3CgMMBgkrBgEE
# AYI3FQYGCCsGAQUFBwMJBggrBgEFBQgCAgYKKwYBBAGCN0ABAQYLKwYBBAGCNwoD
# BAEGCisGAQQBgjcKAwQGCSsGAQQBgjcVBQYKKwYBBAGCNxQCAgYKKwYBBAGCNxQC
# AwYIKwYBBQUHAwMGCisGAQQBgjdbAQEGCisGAQQBgjdbAgEGCisGAQQBgjdbAwEG
# CisGAQQBgjdbBQEGCisGAQQBgjdbBAEGCisGAQQBgjdbBAIwGQYJKwYBBAGCNxQC
# BAweCgBTAHUAYgBDAEEwCwYDVR0PBAQDAgGGMBIGA1UdEwEB/wQIMAYBAf8CAQAw
# HwYDVR0jBBgwFoAUKV5RXmSuNLnrrJwNp4x1AdEJCygwggFoBgNVHR8EggFfMIIB
# WzCCAVegggFToIIBT4YjaHR0cDovL2NybDEuYW1lLmdibC9jcmwvYW1lcm9vdC5j
# cmyGMWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2lpbmZyYS9jcmwvYW1lcm9v
# dC5jcmyGI2h0dHA6Ly9jcmwyLmFtZS5nYmwvY3JsL2FtZXJvb3QuY3JshiNodHRw
# Oi8vY3JsMy5hbWUuZ2JsL2NybC9hbWVyb290LmNybIaBqmxkYXA6Ly8vQ049YW1l
# cm9vdCxDTj1BTUVST09ULENOPUNEUCxDTj1QdWJsaWMlMjBLZXklMjBTZXJ2aWNl
# cyxDTj1TZXJ2aWNlcyxDTj1Db25maWd1cmF0aW9uLERDPUFNRSxEQz1HQkw/Y2Vy
# dGlmaWNhdGVSZXZvY2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3Ry
# aWJ1dGlvblBvaW50MIIBqwYIKwYBBQUHAQEEggGdMIIBmTA3BggrBgEFBQcwAoYr
# aHR0cDovL2NybDEuYW1lLmdibC9haWEvQU1FUk9PVF9hbWVyb290LmNydDBHBggr
# BgEFBQcwAoY7aHR0cDovL2NybC5taWNyb3NvZnQuY29tL3BraWluZnJhL2NlcnRz
# L0FNRVJPT1RfYW1lcm9vdC5jcnQwNwYIKwYBBQUHMAKGK2h0dHA6Ly9jcmwyLmFt
# ZS5nYmwvYWlhL0FNRVJPT1RfYW1lcm9vdC5jcnQwNwYIKwYBBQUHMAKGK2h0dHA6
# Ly9jcmwzLmFtZS5nYmwvYWlhL0FNRVJPT1RfYW1lcm9vdC5jcnQwgaIGCCsGAQUF
# BzAChoGVbGRhcDovLy9DTj1hbWVyb290LENOPUFJQSxDTj1QdWJsaWMlMjBLZXkl
# MjBTZXJ2aWNlcyxDTj1TZXJ2aWNlcyxDTj1Db25maWd1cmF0aW9uLERDPUFNRSxE
# Qz1HQkw/Y0FDZXJ0aWZpY2F0ZT9iYXNlP29iamVjdENsYXNzPWNlcnRpZmljYXRp
# b25BdXRob3JpdHkwDQYJKoZIhvcNAQELBQADggIBACi3Soaajx+kAWjNwgDqkIvK
# AOFkHmS1t0DlzZlpu1ANNfA0BGtck6hEG7g+TpUdVrvxdvPQ5lzU3bGTOBkyhGmX
# oSIlWjKC7xCbbuYegk8n1qj3rTcjiakdbBqqHdF8J+fxv83E2EsZ+StzfCnZXA62
# QCMn6t8mhCWBxpwPXif39Ua32yYHqP0QISAnLTjjcH6bAV3IIk7k5pQ/5NA6qIL8
# yYD6vRjpCMl/3cZOyJD81/5+POLNMx0eCClOfFNxtaD0kJmeThwL4B2hAEpHTeRN
# tB8ib+cze3bvkGNPHyPlSHIuqWoC31x2Gk192SfzFDPV1PqFOcuKjC8049SSBtC1
# X7hyvMqAe4dop8k3u25+odhvDcWdNmimdMWvp/yZ6FyjbGlTxtUqE7iLTLF1eaUL
# SEobAap16hY2N2yTJTISKHzHI4rjsEQlvqa2fj6GLxNj/jC+4LNy+uRmfQXShd30
# lt075qTroz0Nt680pXvVhsRSdNnzW2hfQu2xuOLg8zKGVOD/rr0GgeyhODjKgL2G
# Hxctbb9XaVSDf6ocdB//aDYjiabmWd/WYmy7fQ127KuasMh5nSV2orMcAed8CbIV
# I3NYu+sahT1DRm/BGUN2hSpdsPQeO73wYvp1N7DdLaZyz7XsOCx1quCwQ+bojWVQ
# TmKLGegSoUpZNfmP9MtSMYIVHTCCFRkCAQEwWDBBMRMwEQYKCZImiZPyLGQBGRYD
# R0JMMRMwEQYKCZImiZPyLGQBGRYDQU1FMRUwEwYDVQQDEwxBTUUgQ1MgQ0EgMDEC
# EzYAAAEJd6rskp4/bzsAAQAAAQkwDQYJYIZIAWUDBAIBBQCgga4wGQYJKoZIhvcN
# AQkDMQwGCisGAQQBgjcCAQQwHAYKKwYBBAGCNwIBCzEOMAwGCisGAQQBgjcCARUw
# LwYJKoZIhvcNAQkEMSIEIBXP/GUnAx2YI6fQ2kukX9fotz4xO9+sW+pfjBQf/Uh4
# MEIGCisGAQQBgjcCAQwxNDAyoBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBhodHRw
# Oi8vd3d3Lm1pY3Jvc29mdC5jb20wDQYJKoZIhvcNAQEBBQAEggEAjIrzX4ubTMF3
# UtJ9/mBy442v1PnEtIkMGgzHqfwwo7vhjLV6F8W3xx3um0PFP01liCI5+LIA79Hk
# 62Gz+z31S1/5xAIcUqkvgVVyumglq0+REoy4QrAcYgQtA+Wj0pbsvIRVXi42Fj9/
# +RgPHD0+PE+JFYL9puVA+OCQT3zFxLTwD6GKUBvihhvuji1Lw/D4hxTe5bwqaqGL
# U01hvL5KFVA1HjT4rGzcMEW4QcNg4ZuOwymIIiHevOvKl+PDLOEzVwNdgJ3ccgNH
# UJAj17biIDKrgCLkM5rAnWXG0ffCqaD/r1USGcn3ELHP0uPXVC8MKA/PzZI95Z7H
# hGnRrmAzuqGCEuUwghLhBgorBgEEAYI3AwMBMYIS0TCCEs0GCSqGSIb3DQEHAqCC
# Er4wghK6AgEDMQ8wDQYJYIZIAWUDBAIBBQAwggFRBgsqhkiG9w0BCRABBKCCAUAE
# ggE8MIIBOAIBAQYKKwYBBAGEWQoDATAxMA0GCWCGSAFlAwQCAQUABCA8qxcDBjKp
# P74+HyDvmEskNzWj5OJwqUTDN8tDICGWTwIGXxYBsXBNGBMyMDIwMDgwMzIzNDE0
# Mi4wNTVaMASAAgH0oIHQpIHNMIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
# aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENv
# cnBvcmF0aW9uMSUwIwYDVQQLExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25z
# MSYwJAYDVQQLEx1UaGFsZXMgVFNTIEVTTjo3QkYxLUUzRUEtQjgwODElMCMGA1UE
# AxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAgU2VydmljZaCCDjwwggTxMIID2aADAgEC
# AhMzAAABH04lzawK9LgfAAAAAAEfMA0GCSqGSIb3DQEBCwUAMHwxCzAJBgNVBAYT
# AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYD
# VQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBU
# aW1lLVN0YW1wIFBDQSAyMDEwMB4XDTE5MTExMzIxNDA0MVoXDTIxMDIxMTIxNDA0
# MVowgcoxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
# EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJTAjBgNV
# BAsTHE1pY3Jvc29mdCBBbWVyaWNhIE9wZXJhdGlvbnMxJjAkBgNVBAsTHVRoYWxl
# cyBUU1MgRVNOOjdCRjEtRTNFQS1CODA4MSUwIwYDVQQDExxNaWNyb3NvZnQgVGlt
# ZS1TdGFtcCBTZXJ2aWNlMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA
# pUwPzFb3PxyMf046MLWFCtFwlCpzu1wPi26cp0QYxa19ybbPnDF+1xljQ1p6pXj+
# hPLTQIgsV/xLAnDoKnRGmq37L4eSVSXgMmCTDaZLZUwJi3DysQgdCX8uwo+cnlfG
# fFCZ0OzTIVgjt4wa1oWtqkg0eJ+/h4CeiGfdSfrz9Ds9o/zn8VWpfzscW7vHFjAm
# T3XkATl3Z1UIVxibSDhOGS+fNEb8twd24Rn+qBpitRqVLbJGfLakIMj9cORXXhvY
# WGyCQfyuPmWPKaQ194iZiqWL/s/9CWW5+381++7ORa9aqtOmDyRCTVDvGtLTIZns
# A/WID7SQVxXgq8u5YtsIswIDAQABo4IBGzCCARcwHQYDVR0OBBYEFBWjHwA6MBNj
# BDdlIlsrwTaJ+uDAMB8GA1UdIwQYMBaAFNVjOlyKMZDzQ3t8RhvFM2hahW1VMFYG
# A1UdHwRPME0wS6BJoEeGRWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kvY3Js
# L3Byb2R1Y3RzL01pY1RpbVN0YVBDQV8yMDEwLTA3LTAxLmNybDBaBggrBgEFBQcB
# AQROMEwwSgYIKwYBBQUHMAKGPmh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2kv
# Y2VydHMvTWljVGltU3RhUENBXzIwMTAtMDctMDEuY3J0MAwGA1UdEwEB/wQCMAAw
# EwYDVR0lBAwwCgYIKwYBBQUHAwgwDQYJKoZIhvcNAQELBQADggEBAJjRR8DcZOLK
# YqvJzonPUcQsC9l/Pwu94Mv+7l8X1BW2+zKEqADT9ogJD3Op7cAg3x3x6baEL6qw
# 4hwOCU3dWUEmxAS6hO6Wd8Fdi/oUZlM4qesCiX91DZ5P9xXiEDz1UXQxbZkY/Kge
# gJAI4COyQItpLylGgmmx58X1y5gYUUmlzEHYwCe3UYLCm6AE2qN+/TQ51hylim7B
# NR6s2wqIEJgFLGy11ZgSJOi5tQ5BqLOh+wItYRqroZR0QINNTZ0+KV0qymbanYqI
# MifGbc5M9fNc+f2RNnG4MZvMAfsIUVlq/3MhrWN/kDKgvt7vx0LI/Ofg1lcFPFPj
# wfuKzYtqbEQwggZxMIIEWaADAgECAgphCYEqAAAAAAACMA0GCSqGSIb3DQEBCwUA
# MIGIMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
# UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMTIwMAYDVQQD
# EylNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkgMjAxMDAeFw0x
# MDA3MDEyMTM2NTVaFw0yNTA3MDEyMTQ2NTVaMHwxCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1w
# IFBDQSAyMDEwMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqR0NvHcR
# ijog7PwTl/X6f2mUa3RUENWlCgCChfvtfGhLLF/Fw+Vhwna3PmYrW/AVUycEMR9B
# GxqVHc4JE458YTBZsTBED/FgiIRUQwzXTbg4CLNC3ZOs1nMwVyaCo0UN0Or1R4HN
# vyRgMlhgRvJYR4YyhB50YWeRX4FUsc+TTJLBxKZd0WETbijGGvmGgLvfYfxGwScd
# JGcSchohiq9LZIlQYrFd/XcfPfBXday9ikJNQFHRD5wGPmd/9WbAA5ZEfu/QS/1u
# 5ZrKsajyeioKMfDaTgaRtogINeh4HLDpmc085y9Euqf03GS9pAHBIAmTeM38vMDJ
# RF1eFpwBBU8iTQIDAQABo4IB5jCCAeIwEAYJKwYBBAGCNxUBBAMCAQAwHQYDVR0O
# BBYEFNVjOlyKMZDzQ3t8RhvFM2hahW1VMBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIA
# QwBBMAsGA1UdDwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MB8GA1UdIwQYMBaAFNX2
# VsuP6KJcYmjRPZSQW9fOmhjEMFYGA1UdHwRPME0wS6BJoEeGRWh0dHA6Ly9jcmwu
# bWljcm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY1Jvb0NlckF1dF8yMDEw
# LTA2LTIzLmNybDBaBggrBgEFBQcBAQROMEwwSgYIKwYBBQUHMAKGPmh0dHA6Ly93
# d3cubWljcm9zb2Z0LmNvbS9wa2kvY2VydHMvTWljUm9vQ2VyQXV0XzIwMTAtMDYt
# MjMuY3J0MIGgBgNVHSABAf8EgZUwgZIwgY8GCSsGAQQBgjcuAzCBgTA9BggrBgEF
# BQcCARYxaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL1BLSS9kb2NzL0NQUy9kZWZh
# dWx0Lmh0bTBABggrBgEFBQcCAjA0HjIgHQBMAGUAZwBhAGwAXwBQAG8AbABpAGMA
# eQBfAFMAdABhAHQAZQBtAGUAbgB0AC4gHTANBgkqhkiG9w0BAQsFAAOCAgEAB+aI
# UQ3ixuCYP4FxAz2do6Ehb7Prpsz1Mb7PBeKp/vpXbRkws8LFZslq3/Xn8Hi9x6ie
# JeP5vO1rVFcIK1GCRBL7uVOMzPRgEop2zEBAQZvcXBf/XPleFzWYJFZLdO9CEMiv
# v3/Gf/I3fVo/HPKZeUqRUgCvOA8X9S95gWXZqbVr5MfO9sp6AG9LMEQkIjzP7QOl
# lo9ZKby2/QThcJ8ySif9Va8v/rbljjO7Yl+a21dA6fHOmWaQjP9qYn/dxUoLkSbi
# OewZSnFjnXshbcOco6I8+n99lmqQeKZt0uGc+R38ONiU9MalCpaGpL2eGq4EQoO4
# tYCbIjggtSXlZOz39L9+Y1klD3ouOVd2onGqBooPiRa6YacRy5rYDkeagMXQzafQ
# 732D8OE7cQnfXXSYIghh2rBQHm+98eEA3+cxB6STOvdlR3jo+KhIq/fecn5ha293
# qYHLpwmsObvsxsvYgrRyzR30uIUBHoD7G4kqVDmyW9rIDVWZeodzOwjmmC3qjeAz
# LhIp9cAvVCch98isTtoouLGp25ayp0Kiyc8ZQU3ghvkqmqMRZjDTu3QyS99je/WZ
# ii8bxyGvWbWu3EQ8l1Bx16HSxVXjad5XwdHeMMD9zOZN+w2/XU/pnR4ZOC+8z1gF
# Lu8NoFA12u8JJxzVs341Hgi62jbb01+P3nSISRKhggLOMIICNwIBATCB+KGB0KSB
# zTCByjELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcT
# B1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjElMCMGA1UE
# CxMcTWljcm9zb2Z0IEFtZXJpY2EgT3BlcmF0aW9uczEmMCQGA1UECxMdVGhhbGVz
# IFRTUyBFU046N0JGMS1FM0VBLUI4MDgxJTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1l
# LVN0YW1wIFNlcnZpY2WiIwoBATAHBgUrDgMCGgMVANQvRJa2WvwYqkrR2BYDDosS
# XkkMoIGDMIGApH4wfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24x
# EDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
# bjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTAwDQYJKoZI
# hvcNAQEFBQACBQDi0vTXMCIYDzIwMjAwODA0MDQ0MDU1WhgPMjAyMDA4MDUwNDQw
# NTVaMHcwPQYKKwYBBAGEWQoEATEvMC0wCgIFAOLS9NcCAQAwCgIBAAICHLYCAf8w
# BwIBAAICEZ8wCgIFAOLURlcCAQAwNgYKKwYBBAGEWQoEAjEoMCYwDAYKKwYBBAGE
# WQoDAqAKMAgCAQACAwehIKEKMAgCAQACAwGGoDANBgkqhkiG9w0BAQUFAAOBgQBe
# l1eFLU8o0JldeWXiOI7sHMWxc+BHq4BCryeYW8zvpTmqfLfQ0Em2YndsNsLGHZX6
# a/WQAzKRsMjw3IMrhVFSQm01ZjVQikV+u/4iskzAVmS9FJ0pEm5EGvvlegUAoceU
# ewMiP0Z82TwuDDydjSqEKF96D6nlW8Iu02B0IqiDXzGCAw0wggMJAgEBMIGTMHwx
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1p
# Y3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEwAhMzAAABH04lzawK9LgfAAAAAAEf
# MA0GCWCGSAFlAwQCAQUAoIIBSjAaBgkqhkiG9w0BCQMxDQYLKoZIhvcNAQkQAQQw
# LwYJKoZIhvcNAQkEMSIEIGEqdHOAw5YdfYgqIA7EsXXGrB0LsmJoQGtabrAlGZWv
# MIH6BgsqhkiG9w0BCRACLzGB6jCB5zCB5DCBvQQgqqVw9wBbf/k7/9NxWOfj0eEI
# LScmByNE1X7fVsE+g6UwgZgwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Eg
# MjAxMAITMwAAAR9OJc2sCvS4HwAAAAABHzAiBCA1IDrR8rxktsV4qBUTxytrHCku
# Oae99pf8RO6HzvrSODANBgkqhkiG9w0BAQsFAASCAQBj3m1ec8McxuP/28JeZeR5
# ZY7gj6guG1DUZKYhr1qnWMJSc3mNxbElYxOPqmZGEOYiHNyS3L3Dz/PUrTci8wo6
# yFkLbutudWZwnn+3njdZ9ORbIQ8xM6kKbAVgNAw0yBibANjzmo4wareeRTRpTKCC
# IgG+flyGfk2F4YQfzJom9KOI/pJJXOX1HVVMZDxUxY5iZoCdMkPZX5G8E9vuADCR
# 2StW2UZ3Dts1YRn0EY5TGdPiizqxRs6sUl1Huzm2/tIDsa0R2iZyUG/2e1Gp2mWv
# ER/hE3252eyaMIsqC+LOaGbQYMrXTDQXI9kzdWu5O5t9zGYIH7Ipxdd8mV27V3GV
# SIG # End signature block
