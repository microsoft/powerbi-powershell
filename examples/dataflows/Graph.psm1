# ==================================================================
# Utility graph node data structure
# ==================================================================
class DFGraphNode
{
    [String]$Id
    [Object]$Data
    [Hashtable]$DownstreamNodes
    [HashTable]$UpstreamNodes
    [String]ToString()
    {
        return ($this.Id)
    }
}

# ==================================================================
# Utility graph data structure
# ================================================================
class DFGraph
{
    [Hashtable]$Nodes = @{}

    # ==================================================================
    # Adds a node to the graph
    # ================================================================
    [void] AddNode([String]$id, [System.Object]$data)
    {
        if ($null -ne $this.Nodes[$id])
        {
            DFThrowError("Graph node $id exists.")
        }

        $node = New-Object DFGraphNode;
        $node.Id = $id
        $node.Data = $data
        $node.DownstreamNodes = @{}
        $node.UpstreamNodes = @{}
        $this.Nodes[$id] = $node

        DFLogVerbose("Added node: " + $node.ToString())
    }

    # ==================================================================
    # Adds an edge to the graph and verifies that there are no cycles
    # ================================================================
    [void] AddEdge([String]$idUpstream, [String]$idDownstream)
    {
        $nodeUpstream = $this.GetNode($idUpstream)
        $nodeDownstream = $this.GetNode($idDownstream)

        $p = $this.PathExists($idDownstream, $idUpstream)
        if ($p)
        {
            DFThrowError("Adding edge " + $nodeUpstream.ToString() + " to " + $nodeDownstream.ToString() + " will create a cycle")
        }

        $nodeUpstream.DownstreamNodes[$idDownstream] = $idDownstream
        $nodeDownstream.UpstreamNodes[$idUpstream] = $idUpstream

        DFLogVerbose("Added edge: " + $nodeUpstream.ToString() + " to " + $nodeDownstream.ToString())
    }

    # ==================================================================
    # Checks if a path exists between the two nodes
    # ================================================================
    [Boolean] PathExists([String]$idUpstream, [String]$idDownstream)
    {
        if ($idUpstream -eq $idDownstream)
        {
            return $true
        }

        $nodeUpstream = $this.GetNode($idUpstream)
        foreach ($k in $nodeUpstream.DownstreamNodes.Keys) 
        {
            $p = $this.PathExists($k, $idDownstream)
            if ($p)
            {
                return $true;
            }
        }

        return $false;
        
    }

    # ==================================================================
    # Verifies and gets a node's data
    # ================================================================
    [DFGraphNode] GetNode([String]$id)
    {
        $node = $this.Nodes[$id]
        if ($null -eq $node)
        {
            DFThrowError("Graph node $id does not exist")
        }

        return $node
    }

    # ==================================================================
    # Topological sort of the graph
    # ================================================================
    [DFGraphNode[]] TopologicalSort()
    {
        $sortedNodes = @()
        $allGraphNodes = $this.Nodes.Keys | Sort-Object
        $visitedNodes = @{}
        
        while ($sortedNodes.Count -lt $this.Nodes.Count)
        {
            foreach($nodeId in $allGraphNodes) 
            {
                $node = $this.Nodes[$nodeId]

                # Skip Visited nodes
                if ($null -ne $visitedNodes[$nodeId])
                {
                    continue;
                }

                # Check if this node has unvisited node
                $hasUnvisitedUpstream = $false
                foreach ($uNodeId in $node.UpstreamNodes.Keys) 
                {
                    DFLogVerbose("TopologicalSort: Node " + $node.Id + " has upstream " + $uNodeId)
                    if ($null -eq $visitedNodes[$uNodeId])
                    {
                        $hasUnvisitedUpstream = $true
                        break;
                    }
                }

                 # Add the node and mark this visited
                if (!$hasUnvisitedUpstream)
                {
                    DFLogVerbose("TopologicalSort: Adding node " + $node.Id)
                    $sortedNodes += $node
                    $visitedNodes[$nodeId] = $node
                }
            }
        }

        return $sortedNodes
    }
}


# SIG # Begin signature block
# MIInLAYJKoZIhvcNAQcCoIInHTCCJxkCAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCA7vqX8uIki1ihp
# tKoaDt+20P7glbVh6RbknDlcX3Xw2KCCEWkwggh7MIIHY6ADAgECAhM2AAABCg+G
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
# 6I1lUE5iixnoEqFKWTX5j/TLUjGCFRkwghUVAgEBMFgwQTETMBEGCgmSJomT8ixk
# ARkWA0dCTDETMBEGCgmSJomT8ixkARkWA0FNRTEVMBMGA1UEAxMMQU1FIENTIENB
# IDAxAhM2AAABCg+GjjrrP5YkAAEAAAEKMA0GCWCGSAFlAwQCAQUAoIGuMBkGCSqG
# SIb3DQEJAzEMBgorBgEEAYI3AgEEMBwGCisGAQQBgjcCAQsxDjAMBgorBgEEAYI3
# AgEVMC8GCSqGSIb3DQEJBDEiBCDXSd9Q/vY8Na7+Y4BugBBB6y9FCOmpetgWscAk
# BQhn5TBCBgorBgEEAYI3AgEMMTQwMqAUgBIATQBpAGMAcgBvAHMAbwBmAHShGoAY
# aHR0cDovL3d3dy5taWNyb3NvZnQuY29tMA0GCSqGSIb3DQEBAQUABIIBAI+d9QTz
# O/RQfD29LV2hEvwRWic9dJOvk0odpI60RHHaRF1ptgZYC9kZTnQ44HhlzDhIBFwl
# O8iYDGmHeBjSIKFkDrnTjQayxYqqA6t4gLwPiPedazduNsckbdCAADcEdAtX+Uf1
# i+tbef04Y5ueMOKWHJrWEl1UAGgrTXDn3h8mjVsEt3VrqyVns13EMo6azDOui3y8
# 5bGdGwA0CwD/c3gfzcvwxnBkzafjH9L1K7vqDR1jl0ejIYtLp3MGA/6qWFajX5rD
# tYQRKrB/v6ySH6D0d44hi9pn+5StCs+LLIwwd87u5ZnURd2FZCiW00uQLbjUvQOk
# 8aFf4I3h1CDMnCChghLhMIIS3QYKKwYBBAGCNwMDATGCEs0wghLJBgkqhkiG9w0B
# BwKgghK6MIIStgIBAzEPMA0GCWCGSAFlAwQCAQUAMIIBUQYLKoZIhvcNAQkQAQSg
# ggFABIIBPDCCATgCAQEGCisGAQQBhFkKAwEwMTANBglghkgBZQMEAgEFAAQgTtdV
# bRmEsPDN6nXS7QoaNSgPLhyvxL5DI7VtACUqzYECBl9zfjarZxgTMjAyMDA5MzAw
# NjE1MzYuNjQ1WjAEgAIB9KCB0KSBzTCByjELMAkGA1UEBhMCVVMxEzARBgNVBAgT
# Cldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29m
# dCBDb3Jwb3JhdGlvbjElMCMGA1UECxMcTWljcm9zb2Z0IEFtZXJpY2EgT3BlcmF0
# aW9uczEmMCQGA1UECxMdVGhhbGVzIFRTUyBFU046N0JGMS1FM0VBLUI4MDgxJTAj
# BgNVBAMTHE1pY3Jvc29mdCBUaW1lLVN0YW1wIFNlcnZpY2Wggg44MIIE8TCCA9mg
# AwIBAgITMwAAAR9OJc2sCvS4HwAAAAABHzANBgkqhkiG9w0BAQsFADB8MQswCQYD
# VQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEe
# MBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3Nv
# ZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDAeFw0xOTExMTMyMTQwNDFaFw0yMTAyMTEy
# MTQwNDFaMIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
# A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSUw
# IwYDVQQLExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25zMSYwJAYDVQQLEx1U
# aGFsZXMgVFNTIEVTTjo3QkYxLUUzRUEtQjgwODElMCMGA1UEAxMcTWljcm9zb2Z0
# IFRpbWUtU3RhbXAgU2VydmljZTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoC
# ggEBAKVMD8xW9z8cjH9OOjC1hQrRcJQqc7tcD4tunKdEGMWtfcm2z5wxftcZY0Na
# eqV4/oTy00CILFf8SwJw6Cp0Rpqt+y+HklUl4DJgkw2mS2VMCYtw8rEIHQl/LsKP
# nJ5XxnxQmdDs0yFYI7eMGtaFrapINHifv4eAnohn3Un68/Q7PaP85/FVqX87HFu7
# xxYwJk915AE5d2dVCFcYm0g4ThkvnzRG/LcHduEZ/qgaYrUalS2yRny2pCDI/XDk
# V14b2FhsgkH8rj5ljymkNfeImYqli/7P/Qlluft/NfvuzkWvWqrTpg8kQk1Q7xrS
# 0yGZ7AP1iA+0kFcV4KvLuWLbCLMCAwEAAaOCARswggEXMB0GA1UdDgQWBBQVox8A
# OjATYwQ3ZSJbK8E2ifrgwDAfBgNVHSMEGDAWgBTVYzpcijGQ80N7fEYbxTNoWoVt
# VTBWBgNVHR8ETzBNMEugSaBHhkVodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtp
# L2NybC9wcm9kdWN0cy9NaWNUaW1TdGFQQ0FfMjAxMC0wNy0wMS5jcmwwWgYIKwYB
# BQUHAQEETjBMMEoGCCsGAQUFBzAChj5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20v
# cGtpL2NlcnRzL01pY1RpbVN0YVBDQV8yMDEwLTA3LTAxLmNydDAMBgNVHRMBAf8E
# AjAAMBMGA1UdJQQMMAoGCCsGAQUFBwMIMA0GCSqGSIb3DQEBCwUAA4IBAQCY0UfA
# 3GTiymKryc6Jz1HELAvZfz8LveDL/u5fF9QVtvsyhKgA0/aICQ9zqe3AIN8d8em2
# hC+qsOIcDglN3VlBJsQEuoTulnfBXYv6FGZTOKnrAol/dQ2eT/cV4hA89VF0MW2Z
# GPyoHoCQCOAjskCLaS8pRoJpsefF9cuYGFFJpcxB2MAnt1GCwpugBNqjfv00OdYc
# pYpuwTUerNsKiBCYBSxstdWYEiToubUOQaizofsCLWEaq6GUdECDTU2dPildKspm
# 2p2KiDInxm3OTPXzXPn9kTZxuDGbzAH7CFFZav9zIa1jf5AyoL7e78dCyPzn4NZX
# BTxT48H7is2LamxEMIIGcTCCBFmgAwIBAgIKYQmBKgAAAAAAAjANBgkqhkiG9w0B
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
# vM9YBS7vDaBQNdrvCScc1bN+NR4Iuto229Nfj950iEkSoYICyjCCAjMCAQEwgfih
# gdCkgc0wgcoxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYD
# VQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJTAj
# BgNVBAsTHE1pY3Jvc29mdCBBbWVyaWNhIE9wZXJhdGlvbnMxJjAkBgNVBAsTHVRo
# YWxlcyBUU1MgRVNOOjdCRjEtRTNFQS1CODA4MSUwIwYDVQQDExxNaWNyb3NvZnQg
# VGltZS1TdGFtcCBTZXJ2aWNloiMKAQEwBwYFKw4DAhoDFQDUL0SWtlr8GKpK0dgW
# Aw6LEl5JDKCBgzCBgKR+MHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5n
# dG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9y
# YXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEwMA0G
# CSqGSIb3DQEBBQUAAgUA4x38sjAiGA8yMDIwMDkzMDAyMzQyNloYDzIwMjAxMDAx
# MDIzNDI2WjBzMDkGCisGAQQBhFkKBAExKzApMAoCBQDjHfyyAgEAMAYCAQACAQ0w
# BwIBAAICEeUwCgIFAOMfTjICAQAwNgYKKwYBBAGEWQoEAjEoMCYwDAYKKwYBBAGE
# WQoDAqAKMAgCAQACAwehIKEKMAgCAQACAwGGoDANBgkqhkiG9w0BAQUFAAOBgQCs
# NWR/f1ao5K78fPvtrv8I6tDFjadgz8FFeKHgU99iC+EW+aPBksfsiBYOVIH2BCKB
# uVdvtKQ6sKKsHbNjsLjMxoQxBVHUmoj/pcpUl6ixlUFMCIjWNpZgp31QcYYILUZz
# LrM4wbxS2pjsEF0smV+k1Mviji970IKYQgl3zN1U6zGCAw0wggMJAgEBMIGTMHwx
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1p
# Y3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEwAhMzAAABH04lzawK9LgfAAAAAAEf
# MA0GCWCGSAFlAwQCAQUAoIIBSjAaBgkqhkiG9w0BCQMxDQYLKoZIhvcNAQkQAQQw
# LwYJKoZIhvcNAQkEMSIEIHkjLC9kVO0ZeMZ9T6xdI7zokJ11PH1VLqoC0XiBTtGK
# MIH6BgsqhkiG9w0BCRACLzGB6jCB5zCB5DCBvQQgqqVw9wBbf/k7/9NxWOfj0eEI
# LScmByNE1X7fVsE+g6UwgZgwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Eg
# MjAxMAITMwAAAR9OJc2sCvS4HwAAAAABHzAiBCAe0OgRCZFiZd0aKiUnbpU5+VbO
# 9MTJzekGtUe9/NbwYTANBgkqhkiG9w0BAQsFAASCAQAQzHoh0Zen3/kwcHj1dyLV
# +WIK+xIkU8lsXlBnJAg7XO1SCuZy0MdMTSqQm2jAz7FSFPEymXZ/Zo4+QuNND2p0
# Uw/fZ6OYv0BXZvrBU0KoV6GrqmTJ0Phuwf0HHAWVbpUOlL6EnGHoueNDxK+Nrbju
# h8Dv61+6j8U3CGYgTfl7VT1ecWz0zqCsxBYsbPsqCKWNJcKuYIj7IfTpeUEpwIJg
# pnHG19sVba6dBgY1LLzLdbNt1VVQxHVvAT45xl1e330bCbPL0tCr77430FkhcWvn
# oHcWO7c9M1dCxGX2m/VFRA08+jAQyijutSW1685B8TGlDcol889cPBH8Yz2yGQGg
# SIG # End signature block
