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
# tKoaDt+20P7glbVh6RbknDlcX3Xw2KCCEWUwggh3MIIHX6ADAgECAhM2AAABCXeq
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
# LwYJKoZIhvcNAQkEMSIEINdJ31D+9jw1rv5jgG6AEEHrL0UI6al62BaxwCQFCGfl
# MEIGCisGAQQBgjcCAQwxNDAyoBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBhodHRw
# Oi8vd3d3Lm1pY3Jvc29mdC5jb20wDQYJKoZIhvcNAQEBBQAEggEAirU9nBhSVIuP
# eqA4Mm+00jmH8N7i0jSicvJdnpbuQy7zW6Shdi+/MkwZs3qzo0uZhS8jCjzfcZpg
# LpPy7DxkJg0vdY4l/6R6MogVV4JRX6CiiYvDsMvBpqvob+/8K7cx45ZxeCJ7ajb1
# 02AN94u1/A66uftlINDljQQnLm4knXwohEeGkr2q88vMgk7LOO736ImsJhPsBDpn
# wiWXxLVjTSS4UWA9W7pP4TajiSYxAqGhlZp7qN3yqyC1VjAVAr3SUjzc4Bt5y5e4
# 1ycnyXXlnrC5xWnCJ1iQY9FT2IEcyV8W7rysvJVdm3UtuQCPI3HSjYimqaH3mVYw
# MRnC+hQ+5qGCEuUwghLhBgorBgEEAYI3AwMBMYIS0TCCEs0GCSqGSIb3DQEHAqCC
# Er4wghK6AgEDMQ8wDQYJYIZIAWUDBAIBBQAwggFRBgsqhkiG9w0BCRABBKCCAUAE
# ggE8MIIBOAIBAQYKKwYBBAGEWQoDATAxMA0GCWCGSAFlAwQCAQUABCA8Ai00oSLL
# wyObUuMuZTT4/1NH++RL77dwFDPlR+9hRwIGXxYemx6DGBMyMDIwMDgwMzIzNDAy
# NC4zMzJaMASAAgH0oIHQpIHNMIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2Fz
# aGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENv
# cnBvcmF0aW9uMSUwIwYDVQQLExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25z
# MSYwJAYDVQQLEx1UaGFsZXMgVFNTIEVTTjpFQUNFLUUzMTYtQzkxRDElMCMGA1UE
# AxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAgU2VydmljZaCCDjwwggTxMIID2aADAgEC
# AhMzAAABGrGyX3zHCs0qAAAAAAEaMA0GCSqGSIb3DQEBCwUAMHwxCzAJBgNVBAYT
# AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYD
# VQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBU
# aW1lLVN0YW1wIFBDQSAyMDEwMB4XDTE5MTExMzIxNDAzN1oXDTIxMDIxMTIxNDAz
# N1owgcoxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
# EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJTAjBgNV
# BAsTHE1pY3Jvc29mdCBBbWVyaWNhIE9wZXJhdGlvbnMxJjAkBgNVBAsTHVRoYWxl
# cyBUU1MgRVNOOkVBQ0UtRTMxNi1DOTFEMSUwIwYDVQQDExxNaWNyb3NvZnQgVGlt
# ZS1TdGFtcCBTZXJ2aWNlMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA
# nvl0EF1URiyXWLesOEE1ncMTtuYUCfHoZl16sVM94tiOOdgDWHSij6PDEOIqe7h5
# pMPkWs4Q9uztonPu4k3wRwlTdL/lTFaWm2ZEOCfEyf+FPgQquAnrJprg3sE/1dfO
# 0K6sHhCeAQHAMoN/5RDhjMBYJbCJ8dDD+WmuTfAuIVM7tC91AeQ6GNyyuhQ9f4rE
# NtVEatXpXbnlvZoeMSi+UM/M5X5D24ok+r4yJuHTeppkJpLDm2PW+TXgjs1zMAEH
# rrxNeuRceoPXnBIL2uhm1HcOAXsRvZ52/VtjwMVSB+mIbLX7Rcu31j8K2KvuJJXC
# W0hcqzwun+96k84mOYFHcQIDAQABo4IBGzCCARcwHQYDVR0OBBYEFOZ/F2lB7NiR
# lusURwRIHJ6zVDFqMB8GA1UdIwQYMBaAFNVjOlyKMZDzQ3t8RhvFM2hahW1VMFYG
# A1UdHwRPME0wS6BJoEeGRWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kvY3Js
# L3Byb2R1Y3RzL01pY1RpbVN0YVBDQV8yMDEwLTA3LTAxLmNybDBaBggrBgEFBQcB
# AQROMEwwSgYIKwYBBQUHMAKGPmh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2kv
# Y2VydHMvTWljVGltU3RhUENBXzIwMTAtMDctMDEuY3J0MAwGA1UdEwEB/wQCMAAw
# EwYDVR0lBAwwCgYIKwYBBQUHAwgwDQYJKoZIhvcNAQELBQADggEBAKeDmpN7bXAN
# JNIKV6a00hhpL2w3UR97L6rMh/TFI9YmTmLTM0S30O4Sv+mZQ+y/ixs9Urrtbgta
# FLRwESXsdAzF0l/bFR0l6nEeht3sbqTv/YUpVNWsC5zeByG92MfOEpS+c2fjmioO
# v6ESo/iK6tJK3+z0JibbV56PJIQAajTY2XBRTJkg17V+KI8v5sW+g/rBArondS36
# OrdJIWJb1cohlCsgTESIs5R5UVAQ0vw9oJ5a7QwDx+5knIsTENojpstto3bhnhQv
# j+hOp9xxOo5y3OoRM46BGgi1NH9Z74+TZqKXu2MmxIAKtB3RqxZFaq855L/NfdWu
# gR3+y3NhQKowggZxMIIEWaADAgECAgphCYEqAAAAAAACMA0GCSqGSIb3DQEBCwUA
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
# IFRTUyBFU046RUFDRS1FMzE2LUM5MUQxJTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1l
# LVN0YW1wIFNlcnZpY2WiIwoBATAHBgUrDgMCGgMVAHaytBk+NAhkzfZwpwDdIsy+
# qdTJoIGDMIGApH4wfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24x
# EDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
# bjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTAwDQYJKoZI
# hvcNAQEFBQACBQDi0xHNMCIYDzIwMjAwODA0MDY0NDI5WhgPMjAyMDA4MDUwNjQ0
# MjlaMHcwPQYKKwYBBAGEWQoEATEvMC0wCgIFAOLTEc0CAQAwCgIBAAICFw8CAf8w
# BwIBAAICEcowCgIFAOLUY00CAQAwNgYKKwYBBAGEWQoEAjEoMCYwDAYKKwYBBAGE
# WQoDAqAKMAgCAQACAwehIKEKMAgCAQACAwGGoDANBgkqhkiG9w0BAQUFAAOBgQAj
# h9lHkW2AkZwEeEeWL9pohONW060ImdkcCVc1VLVjEhbSINsVEzCc2R2/pnz1qGaL
# /RPm08tLUSOkNXjB5iuiHw83iVNW8DXuFHidj6GnytA7C0RrUDW94OZ3vr3Fe/NC
# dLM24M2M2wCwAbnY50djPwd/YqzCE/Z/QAjPrhJHFDGCAw0wggMJAgEBMIGTMHwx
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1p
# Y3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEwAhMzAAABGrGyX3zHCs0qAAAAAAEa
# MA0GCWCGSAFlAwQCAQUAoIIBSjAaBgkqhkiG9w0BCQMxDQYLKoZIhvcNAQkQAQQw
# LwYJKoZIhvcNAQkEMSIEIPjgiHLH8w2VVjKv4NbhLHpv60Y4uJfohRJCFKCyj59A
# MIH6BgsqhkiG9w0BCRACLzGB6jCB5zCB5DCBvQQgsWii3MI3S0Xg9jT38i6tT06I
# D8Xjg92BvPGIjd1LZOEwgZgwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0Eg
# MjAxMAITMwAAARqxsl98xwrNKgAAAAABGjAiBCAd8tqkEjW7gYTyQKsLFLTqWTRp
# To49CIqXvopVVyvfLjANBgkqhkiG9w0BAQsFAASCAQCaqzDERfRHLv8iVjnBdKRG
# Hc9gvjLFPccMKkMgEqwnG/YTFhtIp9Bl4jOphjH1KZrnqgGOVL+IIRi3inS6E8Ng
# F3MC4WWT2i4Gv4967hucR2f/qLJvdVOnFKqp+2GvKCjBcEIAOLAKm76AGzV00l7w
# H0noSjzzxSSebouYHZ6+NCyui3AHtJxBKuI1MJRM2v1iXzOvvIaJu3BkxSEDeNRj
# /fjoA8qDYbcZ4/zYqo+uIQqTbkEPk1oXZ1Ru1OgA+vddGdGXAaoAsILsSnXCE+Lh
# AiVudpegiLTzyBVMywcwc0VdVRDhsR0oWPJdS60mlHXqZ8lP9C/4uHGa82u49Jqg
# SIG # End signature block
