/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.IO;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commands.Common.Test
{
    public static class TestUtilities
    {
        public static void ConnectToPowerBI(PowerShell ps)
        {
            ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount))).AddParameter("Environment", "PPE");
            var result = ps.Invoke();
            Assert.IsFalse(ps.HadErrors);
            Assert.IsNotNull(result);
            ps.Commands.Clear();
        }

        public static string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }
    }
}
