using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    [TestClass]
    public class PowerBISettingsTests
    {
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?tabs=basicconfiguration

        [TestMethod]
        public void InValidEnvironmentInJson()
        {
            var testJson = @"
{
  ""Environments"": [
    {
      ""name"": ""Public"",
      ""authority"": ""https://login.windows-ppe.net/common/oauth2/authorize"",
      ""clientId"": ""ea0616ba-638b-4df5-95b9-636659ae5121"",
      ""redirect"": ""urn:ietf:wg:oauth:2.0:oob"",
      ""resource"": ""https://analysis.windows-int.net/powerbi/api"",
      ""override"": ""https://onebox-redirect.analysis.windows-int.net"",
      ""globalService"": ""https://api.powerbi.com""
    },
{
      ""name"": ""NotValid"",
      ""authority"": ""https://login.windows-ppe.net/common/oauth2/authorize"",
      ""clientId"": ""ea0616ba-638b-4df5-95b9-636659ae5121"",
      ""redirect"": ""urn:ietf:wg:oauth:2.0:oob"",
      ""resource"": ""https://analysis.windows-int.net/powerbi/api"",
      ""override"": ""https://onebox-redirect.analysis.windows-int.net"",
      ""globalService"": ""https://api.powerbi.com""
    }
  ],
  ""Settings"": {
    ""Option1"": true
  }
}
";

            var testDir = Directory.GetCurrentDirectory();

            File.WriteAllText(Path.Combine(testDir, "testSettings.json"), testJson);

            var settings = new PowerBISettings(new ConfigurationBuilder().SetBasePath(testDir).AddJsonFile("testSettings.json"));//.AddInMemoryCollection());
            Assert.IsNotNull(settings.Environments);
            Assert.IsNotNull(settings.Settings);
            Assert.IsTrue(settings.Environments.Count == 1);
            Assert.IsTrue(settings.Settings.Count == 1);
        }

        [TestMethod]
        public void ReadDefaultSettingsFile()
        {
            var settings = new PowerBISettings();
            Assert.IsNotNull(settings.Environments);
            Assert.IsNotNull(settings.Settings);
            Assert.IsTrue(settings.Environments.Count != 0);
            Assert.IsTrue(settings.Settings.Count != 0);
        }
    }
}
