/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class GetActivityEventsTests
    {
        private static CmdletInfo GetPowerBIActivityEventsCmdletInfo => new CmdletInfo($"{GetActivityEvents.CmdletVerb}-{GetActivityEvents.CmdletName}", typeof(GetActivityEvents));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIActivityEvents()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, string>()
                {
                    { nameof(GetActivityEvents.StartDateTime), "2019-08-15T20:00:00" },
                    { nameof(GetActivityEvents.EndDateTime), "2019-08-15T22:00:00" },
                    { nameof(GetActivityEvents.Filter), "ViewReport" },
                };

                ps.AddCommand(GetPowerBIActivityEventsCmdletInfo).AddParameters(parameters);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }
    }
}
