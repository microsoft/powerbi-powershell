﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Threading;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    [TestClass]
    public class InvokePowerBIRestMethodTests
    {
        public static CmdletInfo InvokePowerBIRestMethodCmdletInfo { get; } = new CmdletInfo($"{InvokePowerBIRestMethod.CmdletVerb}-{InvokePowerBIRestMethod.CmdletName}", typeof(InvokePowerBIRestMethod));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndInvokePowerBIRestMethodWithOutFile()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBI.Common.Abstractions.PowerBIEnvironmentType.Public);

                ps.AddCommand(InvokePowerBIRestMethodCmdletInfo)
                    .AddParameter("Url", "reports/9dd4146f-3236-4c5c-8141-64fa9f6f0f6c/export")
                    .AddParameter("Method", "Get")
                    .AddParameter("OutFile", ".\\test.pbix");

                ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void InvokePowerBIRestMethod_ModifyHeaders()
        {
            // Arrange
            var initFactory = new TestPowerBICmdletNoClientInitFactory(true);
            var testAuthenticator = initFactory.Authenticator;
            var accessToken = testAuthenticator.Authenticate(profile: null, logger: null, settings: null, queryParameters: null).Result;
            var testHeaderName = "TestExample";
            var testHeaderValue = "Example";
            using (var client = new HttpClient())
            {
                var mock = new MockInvokePowerBIRestMethodCmdlet(initFactory)
                {
                    Headers = new System.Collections.Hashtable()
                    {
                        { testHeaderName, testHeaderValue }
                    }
                };

                // Act
                mock.InvokePopulateClient(accessToken, client);

                // Assert
                Assert.AreEqual(0, client.DefaultRequestHeaders.Accept.Count);
                Assert.IsTrue(client.DefaultRequestHeaders.TryGetValues(testHeaderName, out IEnumerable<string> headerValues));
                Assert.IsNotNull(headerValues);
                Assert.AreEqual(1, headerValues.Count());
                Assert.AreEqual(testHeaderValue, headerValues.First());
            }
        }

        [TestMethod]
        public void InvokePowerBIRestMethod_PositiveTimeout()
        {
            // Arrange
            var initFactory = new TestPowerBICmdletNoClientInitFactory(true);
            var testAuthenticator = initFactory.Authenticator;
            var accessToken = testAuthenticator.Authenticate(profile: null, logger: null, settings: null, queryParameters: null).Result;
            using (var client = new HttpClient())
            {
                var mock = new MockInvokePowerBIRestMethodCmdlet(initFactory)
                {
                    TimeoutSec = 200
                };

                // Act
                mock.InvokePopulateClient(accessToken, client);

                // Assert
                Assert.AreEqual(client.Timeout, TimeSpan.FromSeconds(200));
            }
        }

        [TestMethod]
        public void InvokePowerBIRestMethod_DefaultTimeout()
        {
            // Arrange
            var initFactory = new TestPowerBICmdletNoClientInitFactory(true);
            var testAuthenticator = initFactory.Authenticator;
            var accessToken = testAuthenticator.Authenticate(profile: null, logger: null, settings: null, queryParameters: null).Result;
            using (var client = new HttpClient())
            {
                var mock = new MockInvokePowerBIRestMethodCmdlet(initFactory);

                // Act
                mock.InvokePopulateClient(accessToken, client);

                // Assert
                Assert.AreEqual(client.Timeout, TimeSpan.FromSeconds(100)); // default http client timeout
            }
        }

        [TestMethod]
        public void InvokePowerBIRestMethod_ZeroTimeout()
        {
            // Arrange
            var initFactory = new TestPowerBICmdletNoClientInitFactory(true);
            var testAuthenticator = initFactory.Authenticator;
            var accessToken = testAuthenticator.Authenticate(profile: null, logger: null, settings: null, queryParameters: null).Result;
            using (var client = new HttpClient())
            {
                var mock = new MockInvokePowerBIRestMethodCmdlet(initFactory)
                {
                    TimeoutSec = 0
                };

                // Act
                mock.InvokePopulateClient(accessToken, client);

                // Assert
                Assert.AreEqual(client.Timeout, Timeout.InfiniteTimeSpan); // default http client timeout
            }
        }

        private class MockInvokePowerBIRestMethodCmdlet : InvokePowerBIRestMethod
        {
            public MockInvokePowerBIRestMethodCmdlet() : base() { }

            public MockInvokePowerBIRestMethodCmdlet(IPowerBICmdletInitFactory init) : base(init) { }

            public void InvokePopulateClient(IAccessToken token, HttpClient client)
            {
                base.PopulateClient(token, client);
            }
        }
    }
}
