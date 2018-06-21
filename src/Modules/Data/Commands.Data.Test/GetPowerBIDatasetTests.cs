using System.Management.Automation;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Data
{
    [TestClass]
    public class GetPowerBIDatasetTests
    {
        public static CmdletInfo InvokePowerBIRestMethodCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void GetPowerBIDatasetTest()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(InvokePowerBIRestMethodCmdletInfo).AddParameter("Scope", "Organization");
                var result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }
    }
}
