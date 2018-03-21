/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Commands.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    public static class ProfileTestUtilities
    {
        public static void ConnectToPowerBI(System.Management.Automation.PowerShell ps)
        {
            ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount))).AddParameter("Environment", "PPE");
            var result = ps.Invoke();
            TestUtilities.AssertNoCmdletErrors(ps);
            Assert.IsNotNull(result);
            ps.Commands.Clear();
        }
    }
}
