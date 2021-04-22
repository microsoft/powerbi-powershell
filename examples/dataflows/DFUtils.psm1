#region Log Utilities
# ==================================================================
# Verbose flag
# ==================================================================
[Boolean]$glovalVerbose=$false
[String]$globalLogFile=$null

# ==================================================================
# Function to set global log file
# ==================================================================
function SetLogFile([String]$logFile)
{
	$global:globalLogFile = $logFile
}

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
		
		if ($global:globalLogFile -ne $null)
		{
			$messageInternal | out-file -Filepath $global:globalLogFile -encoding ascii -append
		}
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
# Log message
# ==================================================================
function DFLogHighlight($message)
{
    SafeLog -message $message -color "Green"
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
			return $id
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
# Gets the dataflow id for a given dataflow name in a workspace
# ==================================================================
function GetDataflowIdFromName([String]$workspaceId, [String]$dataflowName)
{
	DFLogMessage("Getting list of dataflows from workspace Id:$workspaceId")
	$myRestResult = Invoke-PowerBIRestMethod -Url "groups/$workspaceId/dataflows" -Method Get | ConvertFrom-Json
	
	foreach ($item in $myRestResult.value) 
	{
		if ($item.name -eq $dataflowName) 
		{
			$id = $item.objectId
			DFLogMessage("Dataflow Name:$dataflowName Id:$id")
			return $id
		}
	}

	DFThrowError("Workspace [$dataflowName] not found.")	
}

# ==================================================================
# Gets the list of dataset ids referred to by a dataflow
# ==================================================================
function GetDependentDatasets([String]$workspaceId, [String]$dataflowId)
{
	$dependentDatasets = @()
	DFLogMessage("Getting dataset to dataflow link in workspace Id:$workspaceId")
	$datasetToDataflowLink = Invoke-PowerBIRestMethod -Url /groups/$workspaceId/datasets/upstreamDataflows -Method Get | ConvertFrom-Json
	foreach ($item in $datasetToDataflowLink.value) 
	{
		if ($item.dataflowObjectId -eq $dataflowId) 
		{
			$dependentDatasets += $item
		}
	}

	DFLogMessage("Dependent dataset count:" + $dependentDatasets.count)
	return $dependentDatasets
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
# Refreshes a dataset
# ==================================================================
function RefreshDataset([String]$workspaceId, [String]$datasetId)
{
	DFLogMessage("Trigerring a refresh for dataset:$datasetId")
	$body = @{
		notifyOption          = "MailOnFailure"
	} | ConvertTo-Json
	Invoke-PowerBIRestMethod -Url /datasets/$datasetId/refreshes -Method Post -Body $body | ConvertFrom-Json
	DFLogMessage("Triggered the refresh successsfully")
}

# ==================================================================
# Refreshes a dataflow and waits for the refresh to complete
# ==================================================================
function RefreshModel([String]$workspaceId, [String]$dataflowId)
{
	$lastTransaction = GetLastTransactionForDataflow $workspaceId $dataflowId
	$lastTransactionId = $null
	if ($null -ne $lastTransaction)
	{
		$lastTransactionId = $lastTransaction.id
		DFLogMessage("Last transaction id:$lastTransactionId")
	}

	DFLogMessage("Trigerring a refresh for dataflow:$dataflowId")
	$body = @{
		notifyOption          = "MailOnFailure"
	} | ConvertTo-Json
	Invoke-PowerBIRestMethod -Url /groups/$workspaceId/dataflows/$dataflowId/refreshes -Method Post -Body $body | ConvertFrom-Json

	DFLogMessage("Waiting for new transaction to start")
	Start-Sleep -Seconds 2.0
	$newTansaction = $null
	$sleepDurationInSeconds = 5
	$maxTimeForPrepareSeconds = 5 * 60
	$maxIters = $maxTimeForPrepareSeconds / $sleepDurationInSeconds
	For ($i=0; $i -le $maxIters; $i++)
	{
		$currentTransaction = GetLastTransactionForDataflow $workspaceId $dataflowId
		if ($null -ne $currentTransaction -and $lastTransactionId -ne $currentTransaction.id)
		{
			$newTansaction = $currentTransaction
			break;
		}

		Start-Sleep -Seconds $sleepDurationInSeconds
	}

	if ($null -eq $newTansaction)
	{
		DFThrowError("Refresh failed. Request timed out")
	}
	
	$txnid = $newTansaction.id
	DFLogMessage("New transaction id:$txnid")

	# Wait for transaction to be completed
	$startDate = Get-Date
	$refreshState = 'inProgress'
	$headers = @{Accept = "application/json, text/plain, */*"}
	$sleepDurationInSeconds = 10
	while($refreshState -eq 'inProgress')
	{
		$refreshStatus = Invoke-PowerBIRestMethod -Url /groups/$workspaceId/dataflows/transactions/$txnid -Method Get -Headers $headers | ConvertFrom-Json
		$refreshState = $refreshStatus.state
		$currentDate = Get-Date
		$ts = New-TimeSpan -Start $startDate -End $currentDate

		if ($refreshState -eq 'failure')
		{
			DFThrowError("Refresh failed after " + $ts.TotalSeconds + " seconds. Message=" + $refreshStatus.exceptionData.message)
		}

		if ($refreshState -eq 'success')
		{
			DFLogMessage("Refresh completed in " + $ts.TotalSeconds + " seconds")
			break
		}

		DFLogMessage("`tRefresh in progress after " + $ts.TotalSeconds + " seconds. Waiting for $sleepDurationInSeconds seconds before polling..")
		Start-Sleep -Seconds $sleepDurationInSeconds
	}
}

function GetLastTransactionForDataflow([String]$workspaceId, [String]$dataflowId)
{
	DFLogVerbose("Getting top 1 transaction for dataflow:$dataflowId")
	$transactions = Invoke-PowerBIRestMethod -Url /groups/$workspaceId/dataflows/$dataflowId/transactions?top=1 -Method Get | ConvertFrom-Json
	if ($null -eq $transactions -or $null -eq $transactions.value -or $transactions.value.length -eq 0)
	{
		return $null
	}

	return $transactions.value[0]
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
# Recreate a folder
# ==================================================================
function RecreateDirectory([String]$directoryName)
{
	If((test-path $directoryName))
    {
        DFLogVerbose("Deleting folder $directoryName")
        Remove-Item -Path $directoryName
	}
	
	DFLogMessage("Creating folder $directoryName")
    New-Item -ItemType Directory -Force -Path $directoryName
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
# MIInMQYJKoZIhvcNAQcCoIInIjCCJx4CAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCAtwhbEs7vipnFn
# qVVL2SIHvc+K7WaZ0WB3BUZ8DvHdSqCCEWkwggh7MIIHY6ADAgECAhM2AAABCg+G
# jjrrP5YkAAEAAAEKMA0GCSqGSIb3DQEBCwUAMEExEzARBgoJkiaJk/IsZAEZFgNH
# QkwxEzARBgoJkiaJk/IsZAEZFgNBTUUxFTATBgNVBAMTDEFNRSBDUyBDQSAwMTAe
# Fw0yMDAyMDkxMzIzNTJaFw0yMTAyMDgxMzIzNTJaMCQxIjAgBgNVBAMTGU1pY3Jv
# c29mdCBBenVyZSBDb2RlIFNpZ24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEK
# AoIBAQCaSxgO08OMIkDBhP5tFtz/NrVIts7g7/GCDLphD1C5ebj5LwRbJnDCZAJb
# YJcOOD8+1Hf+nbP0a+E48D89FZ3+3Wlz4LKe1i+y9EhBvgvS/7xk8PgJ5edxpxwA
# sZ+QEZ6My08M39J0eVu3hLCFYkEvXZiJx8vWtwM9QhzpC95jXhFbaW1J698DzlHJ
# mpXN8vnx113KHFYGYBOgIScOKwZRpqQKp8qrWMLYjrqd8Yauy+AnwQ1dwc/HXr+I
# vY8R857711Lr3w0V/d+pSyDntkLFyh7wnvbqp1H408H8LA53CxR++D1p0qTMQ9u5
# /7Aq1PgUBIdEPt+9q/l4XqYUK4JHAgMBAAGjggWHMIIFgzApBgkrBgEEAYI3FQoE
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
# aG9yaXR5MB0GA1UdDgQWBBSbi7b9oM/Zs0NL/jWj2iR9gUS7JTAOBgNVHQ8BAf8E
# BAMCB4AwVAYDVR0RBE0wS6RJMEcxLTArBgNVBAsTJE1pY3Jvc29mdCBJcmVsYW5k
# IE9wZXJhdGlvbnMgTGltaXRlZDEWMBQGA1UEBRMNMjM2MTY3KzQ1Nzc5MDCCAdQG
# A1UdHwSCAcswggHHMIIBw6CCAb+gggG7hjxodHRwOi8vY3JsLm1pY3Jvc29mdC5j
# b20vcGtpaW5mcmEvQ1JML0FNRSUyMENTJTIwQ0ElMjAwMS5jcmyGLmh0dHA6Ly9j
# cmwxLmFtZS5nYmwvY3JsL0FNRSUyMENTJTIwQ0ElMjAwMS5jcmyGLmh0dHA6Ly9j
# cmwyLmFtZS5nYmwvY3JsL0FNRSUyMENTJTIwQ0ElMjAwMS5jcmyGLmh0dHA6Ly9j
# cmwzLmFtZS5nYmwvY3JsL0FNRSUyMENTJTIwQ0ElMjAwMS5jcmyGLmh0dHA6Ly9j
# cmw0LmFtZS5nYmwvY3JsL0FNRSUyMENTJTIwQ0ElMjAwMS5jcmyGgbpsZGFwOi8v
# L0NOPUFNRSUyMENTJTIwQ0ElMjAwMSxDTj1CWTJQS0lDU0NBMDEsQ049Q0RQLENO
# PVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3Vy
# YXRpb24sREM9QU1FLERDPUdCTD9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0P2Jh
# c2U/b2JqZWN0Q2xhc3M9Y1JMRGlzdHJpYnV0aW9uUG9pbnQwHwYDVR0jBBgwFoAU
# G2aiGfyb66XahI8YmOkQpMN7kr0wHwYDVR0lBBgwFgYKKwYBBAGCN1sBAQYIKwYB
# BQUHAwMwDQYJKoZIhvcNAQELBQADggEBAHoJpCl2fKUhm2GAnH5+ktQ13RZCV75r
# Cqq5fBClbh2OtSoWgjjeRHkXUk9YP8WucQWR7vlHXBM2ZoIaSvuoI4LeLZbr7Cqp
# 13EA1E2OQe6mE5zXlOLAYhwrW6ChLgDkiOnRlqLrkKeUtzL7GzBsSfER+D/Xawcz
# gd8D2T6sd7YvJ+GqfJ/ZM4j8Z3gLNyaHYRRX+8bkM+aQFdh05Pj8X0z6qpTBb6g4
# Pymllq2WHP7hnoqwSNeR7hg6VOO8k+1wr59ZDGvKvHP1cdg2ZfZZsHgd3Bh1YW42
# xBnugHRF46knbxwgFCACriWe7AMY6hO40L0ocjPFkf163wWi1LCBI4AwggjmMIIG
# zqADAgECAhMfAAAAFLTFH8bygL5xAAAAAAAUMA0GCSqGSIb3DQEBCwUAMDwxEzAR
# BgoJkiaJk/IsZAEZFgNHQkwxEzARBgoJkiaJk/IsZAEZFgNBTUUxEDAOBgNVBAMT
# B2FtZXJvb3QwHhcNMTYwOTE1MjEzMzAzWhcNMjEwOTE1MjE0MzAzWjBBMRMwEQYK
# CZImiZPyLGQBGRYDR0JMMRMwEQYKCZImiZPyLGQBGRYDQU1FMRUwEwYDVQQDEwxB
# TUUgQ1MgQ0EgMDEwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDVV4EC
# 1vn60PcbgLndN80k3GZh/OGJcq0pDNIbG5q/rrRtNLVUR4MONKcWGyaeVvoaQ8J5
# iYInBaBkaz7ehYnzJp3f/9Wg/31tcbxrPNMmZPY8UzXIrFRdQmCLsj3LcLiWX8BN
# 8HBsYZFcP7Y92R2VWnEpbN40Q9XBsK3FaNSEevoRzL1Ho7beP7b9FJlKB/Nhy0PM
# NaE1/Q+8Y9+WbfU9KTj6jNxrffv87O7T6doMqDmL/MUeF9IlmSrl088boLzAOt2L
# AeHobkgasx3ZBeea8R+O2k+oT4bwx5ZuzNpbGXESNAlALo8HCf7xC3hWqVzRqbdn
# d8HDyTNG6c6zwyf/AgMBAAGjggTaMIIE1jAQBgkrBgEEAYI3FQEEAwIBATAjBgkr
# BgEEAYI3FQIEFgQUkfwzzkKe9pPm4n1U1wgYu7jXcWUwHQYDVR0OBBYEFBtmohn8
# m+ul2oSPGJjpEKTDe5K9MIIBBAYDVR0lBIH8MIH5BgcrBgEFAgMFBggrBgEFBQcD
# AQYIKwYBBQUHAwIGCisGAQQBgjcUAgEGCSsGAQQBgjcVBgYKKwYBBAGCNwoDDAYJ
# KwYBBAGCNxUGBggrBgEFBQcDCQYIKwYBBQUIAgIGCisGAQQBgjdAAQEGCysGAQQB
# gjcKAwQBBgorBgEEAYI3CgMEBgkrBgEEAYI3FQUGCisGAQQBgjcUAgIGCisGAQQB
# gjcUAgMGCCsGAQUFBwMDBgorBgEEAYI3WwEBBgorBgEEAYI3WwIBBgorBgEEAYI3
# WwMBBgorBgEEAYI3WwUBBgorBgEEAYI3WwQBBgorBgEEAYI3WwQCMBkGCSsGAQQB
# gjcUAgQMHgoAUwB1AGIAQwBBMAsGA1UdDwQEAwIBhjASBgNVHRMBAf8ECDAGAQH/
# AgEAMB8GA1UdIwQYMBaAFCleUV5krjS566ycDaeMdQHRCQsoMIIBaAYDVR0fBIIB
# XzCCAVswggFXoIIBU6CCAU+GI2h0dHA6Ly9jcmwxLmFtZS5nYmwvY3JsL2FtZXJv
# b3QuY3JshjFodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtpaW5mcmEvY3JsL2Ft
# ZXJvb3QuY3JshiNodHRwOi8vY3JsMi5hbWUuZ2JsL2NybC9hbWVyb290LmNybIYj
# aHR0cDovL2NybDMuYW1lLmdibC9jcmwvYW1lcm9vdC5jcmyGgapsZGFwOi8vL0NO
# PWFtZXJvb3QsQ049QU1FUk9PVCxDTj1DRFAsQ049UHVibGljJTIwS2V5JTIwU2Vy
# dmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1BTUUsREM9R0JM
# P2NlcnRpZmljYXRlUmV2b2NhdGlvbkxpc3Q/YmFzZT9vYmplY3RDbGFzcz1jUkxE
# aXN0cmlidXRpb25Qb2ludDCCAasGCCsGAQUFBwEBBIIBnTCCAZkwNwYIKwYBBQUH
# MAKGK2h0dHA6Ly9jcmwxLmFtZS5nYmwvYWlhL0FNRVJPT1RfYW1lcm9vdC5jcnQw
# RwYIKwYBBQUHMAKGO2h0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2lpbmZyYS9j
# ZXJ0cy9BTUVST09UX2FtZXJvb3QuY3J0MDcGCCsGAQUFBzAChitodHRwOi8vY3Js
# Mi5hbWUuZ2JsL2FpYS9BTUVST09UX2FtZXJvb3QuY3J0MDcGCCsGAQUFBzAChito
# dHRwOi8vY3JsMy5hbWUuZ2JsL2FpYS9BTUVST09UX2FtZXJvb3QuY3J0MIGiBggr
# BgEFBQcwAoaBlWxkYXA6Ly8vQ049YW1lcm9vdCxDTj1BSUEsQ049UHVibGljJTIw
# S2V5JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1B
# TUUsREM9R0JMP2NBQ2VydGlmaWNhdGU/YmFzZT9vYmplY3RDbGFzcz1jZXJ0aWZp
# Y2F0aW9uQXV0aG9yaXR5MA0GCSqGSIb3DQEBCwUAA4ICAQAot0qGmo8fpAFozcIA
# 6pCLygDhZB5ktbdA5c2ZabtQDTXwNARrXJOoRBu4Pk6VHVa78Xbz0OZc1N2xkzgZ
# MoRpl6EiJVoygu8Qm27mHoJPJ9ao9603I4mpHWwaqh3RfCfn8b/NxNhLGfkrc3wp
# 2VwOtkAjJ+rfJoQlgcacD14n9/VGt9smB6j9ECEgJy0443B+mwFdyCJO5OaUP+TQ
# OqiC/MmA+r0Y6QjJf93GTsiQ/Nf+fjzizTMdHggpTnxTcbWg9JCZnk4cC+AdoQBK
# R03kTbQfIm/nM3t275BjTx8j5UhyLqlqAt9cdhpNfdkn8xQz1dT6hTnLiowvNOPU
# kgbQtV+4crzKgHuHaKfJN7tufqHYbw3FnTZopnTFr6f8mehco2xpU8bVKhO4i0yx
# dXmlC0hKGwGqdeoWNjdskyUyEih8xyOK47BEJb6mtn4+hi8TY/4wvuCzcvrkZn0F
# 0oXd9JbdO+ak66M9DbevNKV71YbEUnTZ81toX0Ltsbji4PMyhlTg/669BoHsoTg4
# yoC9hh8XLW2/V2lUg3+qHHQf/2g2I4mm5lnf1mJsu30NduyrmrDIeZ0ldqKzHAHn
# fAmyFSNzWLvrGoU9Q0ZvwRlDdoUqXbD0Hju98GL6dTew3S2mcs+17DgsdargsEPm
# 6I1lUE5iixnoEqFKWTX5j/TLUjGCFR4wghUaAgEBMFgwQTETMBEGCgmSJomT8ixk
# ARkWA0dCTDETMBEGCgmSJomT8ixkARkWA0FNRTEVMBMGA1UEAxMMQU1FIENTIENB
# IDAxAhM2AAABCg+GjjrrP5YkAAEAAAEKMA0GCWCGSAFlAwQCAQUAoIGuMBkGCSqG
# SIb3DQEJAzEMBgorBgEEAYI3AgEEMBwGCisGAQQBgjcCAQsxDjAMBgorBgEEAYI3
# AgEVMC8GCSqGSIb3DQEJBDEiBCBZx6SFdKiTxZPT8MujUo17GLVfXc6qDNcS3P3s
# 5VtnTDBCBgorBgEEAYI3AgEMMTQwMqAUgBIATQBpAGMAcgBvAHMAbwBmAHShGoAY
# aHR0cDovL3d3dy5taWNyb3NvZnQuY29tMA0GCSqGSIb3DQEBAQUABIIBAG5UsnWI
# OVqCWkQYeKEkaVgdvdlMO1p3EVWYwR4f5f6SuCDw/d3qtxjWKH7LbKad/I6ae8DA
# wcjpPYf/2lIuJr0b8j8FKQxbOjPvqKVTw/1C9JM3NYQ1IYLtfHwqX8Q5CNzyAE56
# 8LAIk3ktgRURf9ur0gVB47NYt1NQ/lGEuyPe9vEgEcKQabX9azwDmAMySxDeQLCi
# UrHdX+lcE+njGK2970Hy+FtGVQPM35r3uu+Sexd8p4MkRX5QT7ow/iFflVweJQvz
# OA7op904G7yrFj8E9Og8JDpvpUZLHVuB5LxzkWFJOQt/Z7FHcsfNrzwfrssqCfI3
# uzW2UsXwRpTI7M6hghLmMIIS4gYKKwYBBAGCNwMDATGCEtIwghLOBgkqhkiG9w0B
# BwKgghK/MIISuwIBAzEPMA0GCWCGSAFlAwQCAQUAMIIBUQYLKoZIhvcNAQkQAQSg
# ggFABIIBPDCCATgCAQEGCisGAQQBhFkKAwEwMTANBglghkgBZQMEAgEFAAQgOIGT
# Qb7rP9LTwHal69oGGW6WovcoOs+02lqu4KlvgQkCBl9zfLRhnxgTMjAyMDA5MzAw
# NjE1MTYuMjY4WjAEgAIB9KCB0KSBzTCByjELMAkGA1UEBhMCVVMxEzARBgNVBAgT
# Cldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29m
# dCBDb3Jwb3JhdGlvbjElMCMGA1UECxMcTWljcm9zb2Z0IEFtZXJpY2EgT3BlcmF0
# aW9uczEmMCQGA1UECxMdVGhhbGVzIFRTUyBFU046RDZCRC1FM0U3LTE2ODUxJTAj
# BgNVBAMTHE1pY3Jvc29mdCBUaW1lLVN0YW1wIFNlcnZpY2Wggg49MIIE8TCCA9mg
# AwIBAgITMwAAAR4OvOVLFqIDGwAAAAABHjANBgkqhkiG9w0BAQsFADB8MQswCQYD
# VQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEe
# MBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3Nv
# ZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDAeFw0xOTExMTMyMTQwNDBaFw0yMTAyMTEy
# MTQwNDBaMIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
# A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSUw
# IwYDVQQLExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25zMSYwJAYDVQQLEx1U
# aGFsZXMgVFNTIEVTTjpENkJELUUzRTctMTY4NTElMCMGA1UEAxMcTWljcm9zb2Z0
# IFRpbWUtU3RhbXAgU2VydmljZTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoC
# ggEBAM4TtxgQovz18FyurO38G3WqlV+etLFjCViCzevcL+0aVl4USidzKo5r5FFg
# ZB9b6ncAkfAJxYf6xmQ42HDmtpju+cK2O24q3xu+o1DRp7DFd3261HnBZVRfnEoR
# 7PAIh9eenBq+LFH4Z3pArL3U1y8TwVdBU91WEOvcUyLM6qSpyHIdiuPgz0uC3FuS
# IPJxrGxq/dfrxO21zCkFwwKfahsVJmMJpRXMdsavoR+gvTdN5pvHRZmsR7bHtBPR
# mRhAEJiYlLVRdBIBVWOpvXCcxevv7Ufx8cut3X920zYOxH8NfCfASjP1nVSmt5+W
# mHd3VXYhtX3Mo559eCn8gHZpFLsCAwEAAaOCARswggEXMB0GA1UdDgQWBBSMEyjn
# kXhG4Ev7fps/2a8n2maKWzAfBgNVHSMEGDAWgBTVYzpcijGQ80N7fEYbxTNoWoVt
# VTBWBgNVHR8ETzBNMEugSaBHhkVodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtp
# L2NybC9wcm9kdWN0cy9NaWNUaW1TdGFQQ0FfMjAxMC0wNy0wMS5jcmwwWgYIKwYB
# BQUHAQEETjBMMEoGCCsGAQUFBzAChj5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20v
# cGtpL2NlcnRzL01pY1RpbVN0YVBDQV8yMDEwLTA3LTAxLmNydDAMBgNVHRMBAf8E
# AjAAMBMGA1UdJQQMMAoGCCsGAQUFBwMIMA0GCSqGSIb3DQEBCwUAA4IBAQAuZNyO
# dZYjkIITIlQNJeh2NIc83bDeiIBFIO+DmMjbsfaGPuv0L7/54xTmR+TMj2ZMn/eb
# W5pTJoa9Y75oZd8XqFO/KEYBCjahyXC5Bxw+pWqT70BGsg+m0IdGYaFADJYQm6NW
# C1atY38q0oscfoZYgGR4THJIkXZpN+7uPr1yA/PkMNK+XdSaCFQGXW5NdSH/Qx5C
# ySF3B8ngEpRos7aoABeaVAfja1FVqxrSo1gx0+bvEXVhBWWvUQGe+b2VQdNpvQ2p
# UX4S7qRufctSzSiAeBaYECaRCNY5rK1ovLAwiEd3Bg7KntLBolQfHr1w/Vc2s52i
# ScaFReh04dJdfiFtMIIGcTCCBFmgAwIBAgIKYQmBKgAAAAAAAjANBgkqhkiG9w0B
# AQsFADCBiDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNV
# BAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAG
# A1UEAxMpTWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5IDIwMTAw
# HhcNMTAwNzAxMjEzNjU1WhcNMjUwNzAxMjE0NjU1WjB8MQswCQYDVQQGEwJVUzET
# MBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMV
# TWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1T
# dGFtcCBQQ0EgMjAxMDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKkd
# Dbx3EYo6IOz8E5f1+n9plGt0VBDVpQoAgoX77XxoSyxfxcPlYcJ2tz5mK1vwFVMn
# BDEfQRsalR3OCROOfGEwWbEwRA/xYIiEVEMM1024OAizQt2TrNZzMFcmgqNFDdDq
# 9UeBzb8kYDJYYEbyWEeGMoQedGFnkV+BVLHPk0ySwcSmXdFhE24oxhr5hoC732H8
# RsEnHSRnEnIaIYqvS2SJUGKxXf13Hz3wV3WsvYpCTUBR0Q+cBj5nf/VmwAOWRH7v
# 0Ev9buWayrGo8noqCjHw2k4GkbaICDXoeByw6ZnNPOcvRLqn9NxkvaQBwSAJk3jN
# /LzAyURdXhacAQVPIk0CAwEAAaOCAeYwggHiMBAGCSsGAQQBgjcVAQQDAgEAMB0G
# A1UdDgQWBBTVYzpcijGQ80N7fEYbxTNoWoVtVTAZBgkrBgEEAYI3FAIEDB4KAFMA
# dQBiAEMAQTALBgNVHQ8EBAMCAYYwDwYDVR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAW
# gBTV9lbLj+iiXGJo0T2UkFvXzpoYxDBWBgNVHR8ETzBNMEugSaBHhkVodHRwOi8v
# Y3JsLm1pY3Jvc29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9NaWNSb29DZXJBdXRf
# MjAxMC0wNi0yMy5jcmwwWgYIKwYBBQUHAQEETjBMMEoGCCsGAQUFBzAChj5odHRw
# Oi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY1Jvb0NlckF1dF8yMDEw
# LTA2LTIzLmNydDCBoAYDVR0gAQH/BIGVMIGSMIGPBgkrBgEEAYI3LgMwgYEwPQYI
# KwYBBQUHAgEWMWh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9QS0kvZG9jcy9DUFMv
# ZGVmYXVsdC5odG0wQAYIKwYBBQUHAgIwNB4yIB0ATABlAGcAYQBsAF8AUABvAGwA
# aQBjAHkAXwBTAHQAYQB0AGUAbQBlAG4AdAAuIB0wDQYJKoZIhvcNAQELBQADggIB
# AAfmiFEN4sbgmD+BcQM9naOhIW+z66bM9TG+zwXiqf76V20ZMLPCxWbJat/15/B4
# vceoniXj+bzta1RXCCtRgkQS+7lTjMz0YBKKdsxAQEGb3FwX/1z5Xhc1mCRWS3Tv
# QhDIr79/xn/yN31aPxzymXlKkVIArzgPF/UveYFl2am1a+THzvbKegBvSzBEJCI8
# z+0DpZaPWSm8tv0E4XCfMkon/VWvL/625Y4zu2JfmttXQOnxzplmkIz/amJ/3cVK
# C5Em4jnsGUpxY517IW3DnKOiPPp/fZZqkHimbdLhnPkd/DjYlPTGpQqWhqS9nhqu
# BEKDuLWAmyI4ILUl5WTs9/S/fmNZJQ96LjlXdqJxqgaKD4kWumGnEcua2A5HmoDF
# 0M2n0O99g/DhO3EJ3110mCIIYdqwUB5vvfHhAN/nMQekkzr3ZUd46PioSKv33nJ+
# YWtvd6mBy6cJrDm77MbL2IK0cs0d9LiFAR6A+xuJKlQ5slvayA1VmXqHczsI5pgt
# 6o3gMy4SKfXAL1QnIffIrE7aKLixqduWsqdCosnPGUFN4Ib5KpqjEWYw07t0Mkvf
# Y3v1mYovG8chr1m1rtxEPJdQcdeh0sVV42neV8HR3jDA/czmTfsNv11P6Z0eGTgv
# vM9YBS7vDaBQNdrvCScc1bN+NR4Iuto229Nfj950iEkSoYICzzCCAjgCAQEwgfih
# gdCkgc0wgcoxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYD
# VQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJTAj
# BgNVBAsTHE1pY3Jvc29mdCBBbWVyaWNhIE9wZXJhdGlvbnMxJjAkBgNVBAsTHVRo
# YWxlcyBUU1MgRVNOOkQ2QkQtRTNFNy0xNjg1MSUwIwYDVQQDExxNaWNyb3NvZnQg
# VGltZS1TdGFtcCBTZXJ2aWNloiMKAQEwBwYFKw4DAhoDFQA5yQbj7emrMRP+jjdY
# uspZjMqw3KCBgzCBgKR+MHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5n
# dG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9y
# YXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEwMA0G
# CSqGSIb3DQEBBQUAAgUA4x37LDAiGA8yMDIwMDkzMDAyMjc1NloYDzIwMjAxMDAx
# MDIyNzU2WjB4MD4GCisGAQQBhFkKBAExMDAuMAoCBQDjHfssAgEAMAsCAQACAwS1
# 8AIB/zAHAgEAAgIRyjAKAgUA4x9MrAIBADA2BgorBgEEAYRZCgQCMSgwJjAMBgor
# BgEEAYRZCgMCoAowCAIBAAIDB6EgoQowCAIBAAIDAYagMA0GCSqGSIb3DQEBBQUA
# A4GBAHlJnI/DFF/6HgTSvCWhdqDwdifI/Tz2vkYS1vfpnlw8hZSJE5uIA/qzND63
# p4xart9dhA3aieRbhQv3FUswgJNVO/ZlXukVJNwvXNUuVVC20C97nhA0/7aUgkl1
# odWKUd1jR7WXB3kU6r37Eu6ZviEJNp6EW94E84X+CuY4fNWgMYIDDTCCAwkCAQEw
# gZMwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcT
# B1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UE
# AxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTACEzMAAAEeDrzlSxaiAxsA
# AAAAAR4wDQYJYIZIAWUDBAIBBQCgggFKMBoGCSqGSIb3DQEJAzENBgsqhkiG9w0B
# CRABBDAvBgkqhkiG9w0BCQQxIgQgyBuMA0Cvo7vxwLjOPhRK9bts+e4dbSHx6ZwY
# 1woc57EwgfoGCyqGSIb3DQEJEAIvMYHqMIHnMIHkMIG9BCBzO+RYw99xOlHlvaef
# PKE3cS3NJdWU8foiBBwPjdZfRzCBmDCBgKR+MHwxCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1w
# IFBDQSAyMDEwAhMzAAABHg685UsWogMbAAAAAAEeMCIEIBPJLLCdX/jUsCZl12yn
# 5xwTWlHR3VCbrwaOXPIaF294MA0GCSqGSIb3DQEBCwUABIIBAIMlp4l6mLW1jBWj
# YYnMkZp8SZ3AFmaoAT8qThXlaj1N9KYgFZjzI3DgiMzvW1GsEFH8Lfz2RXsazRgt
# NIQ6oMltjS4OJkbOo2eCW0X1suGIsNcEYksTPU6OCt3NMH37HNNv3+PjjMI8x/qU
# kBgZvtSAokQTKOTM9lrUXzaBcPebim3Fz5I6D2g8NlOzs/G4wBQzKKxxUkqdFI/J
# AN76TTwiQM4vTANMC+4SZ09UH3ExrneDKWyzmbYYbYgxs0LvyuY7X2K5+uBZdfxX
# pqKylAa5Gi+K99ybz9anlmJR+CawskMENY9aeYcjAhQWTPOd/l5WxI0PZCg3SnHS
# mNFz+TI=
# SIG # End signature block
