## 1.0.880
Updates to existing cmdlets:
- Add-PowerBIWorkspaceUser: updated parameter -AccessRight to accept Viewer as input.
- Get-PowerBIWorkspace: Display UserPrincipalName properly
- Update security protocol to use TLS 1.2

## 1.0.867
Updates to existing cmdlets:
- Connect-PowerBIServiceAccount: added parameters -DiscoverUrl (string) to specify a custom service discovery url and -CustomEnvironment (string)
to specify the environment to choose.
- Invoke-PowerBIRestMethod: added parameter -TimeoutSec (int) to specify a client timeout in seconds.

## Previous releases are not included in this change log