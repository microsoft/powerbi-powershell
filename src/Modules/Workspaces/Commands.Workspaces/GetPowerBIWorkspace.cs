using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(ODataResponseListGroup))]
    public class GetPowerBIWorkspace : PowerBICmdlet
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        protected override void ExecuteCmdlet()
        {
            var token = this.Authenticator.Authenticate(this.Profile.Environment, this.Logger, this.Settings);
            var client = new PowerBIClient(new TokenCredentials(token.AccessToken));

            var workspaces = client.Groups.GetGroups();
            this.Logger.WriteObject(workspaces);
        }
    }
}