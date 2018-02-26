using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Commands.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    [TestClass]
    public class ConnectPowerBIServiceAccountTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndInteractiveLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount)));
                var result = ps.Invoke();
                Assert.IsTrue(result.Count == 1);
                Assert.IsTrue(result[0].BaseObject is PowerBIProfile);
                var profile = result[0].BaseObject as PowerBIProfile;
                Assert.IsNotNull(profile.Environment);
                Assert.IsNotNull(profile.UserName);
                Assert.IsNotNull(profile.TenantId);
            }
        }
    }
}
