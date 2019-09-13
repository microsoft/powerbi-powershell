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
using Microsoft.PowerBI.Common.Api.ActivityEvent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class GetPowerBIActivityEventsTests
    {
        private static CmdletInfo GetPowerBIActivityEventsCmdletInfo => new CmdletInfo($"{GetPowerBIActivityEvents.CmdletVerb}-{GetPowerBIActivityEvents.CmdletName}", typeof(GetPowerBIActivityEvents));
        private static string StartDateTime = "2019-08-15T20:00:00Z";
        private static string EndDateTime = "2019-08-15T22:00:00Z";

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIActivityEvents()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIActivityEvents.StartDateTime), StartDateTime },
                    { nameof(GetPowerBIActivityEvents.EndDateTime), EndDateTime },
                    { nameof(GetPowerBIActivityEvents.ActivityType), "ViewReport" },
                };

                ps.AddCommand(GetPowerBIActivityEventsCmdletInfo).AddParameters(parameters);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndGetPowerBIActivityEventsWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIActivityEvents.StartDateTime), StartDateTime },
                    { nameof(GetPowerBIActivityEvents.EndDateTime), EndDateTime },
                    { nameof(GetPowerBIActivityEvents.ActivityType), "ViewReport" },
                    { nameof(GetPowerBIActivityEvents.User), "admin@alangria.info" },
                };

                ps.AddCommand(GetPowerBIActivityEventsCmdletInfo).AddParameters(parameters);

                // Act
                var result = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void GetPowerBIActivityEvents_NonInteractive()
        {
            // Arrange
            object obj1 = new object();
            object obj2 = new object();
            object obj3 = new object();
            IList<object> ActivityEventEntities = new List<object>
            {
                obj1,
                obj2,
                obj3
            };

            var activityEventResponse = new ActivityEventResponse();
            activityEventResponse.ActivityEventEntities = ActivityEventEntities;
            activityEventResponse.ContinuationToken = null;

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIActivityEvents($"'{StartDateTime}'", $"'{EndDateTime}'", null, null)).Returns(activityEventResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIActivityEvents(initFactory)
            {
                StartDateTime = StartDateTime,
                EndDateTime = EndDateTime,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(activityEventResponse, initFactory);
        }

        [TestMethod]
        public void GetPowerBIActivityEventsWithInvalidStartDateTime()
        {
            // Arrange
            var activityEventResponse = new ActivityEventResponse
            {
                ActivityEventEntities = new List<object>
                {
                    new object()
                },
                ContinuationToken = "next-page"
            };

            string invalidStartDateTime = "Some-invalid-startDateTime";
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIActivityEvents($"'{invalidStartDateTime}'", $"'{EndDateTime}'", null, null)).Returns(activityEventResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIActivityEvents(initFactory)
            {
                StartDateTime = invalidStartDateTime,
                EndDateTime = EndDateTime,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertGetActivityEventsNeverCalled(client, initFactory);
        }

        [TestMethod]
        public void GetPowerBIActivityEventsWithInvalidEndDateTime()
        {
            // Arrange
            var activityEventResponse = new ActivityEventResponse
            {
                ActivityEventEntities = new List<object>
                {
                    new object()
                },
                ContinuationToken = "next-page"
            };

            string invalidEndDateTime = "Some-invalid-endDateTime";
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIActivityEvents($"'{StartDateTime}'", $"'{invalidEndDateTime}'", null, null)).Returns(activityEventResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIActivityEvents(initFactory)
            {
                StartDateTime = StartDateTime,
                EndDateTime = invalidEndDateTime,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertGetActivityEventsNeverCalled(client, initFactory);
        }


        private static void AssertExpectedUnitTestResults(ActivityEventResponse expectedResponse, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            List<object> actualResponse = JsonConvert.DeserializeObject<List<object>>(results[0].ToString());
            Assert.AreEqual(expectedResponse.ActivityEventEntities.Count(), actualResponse.Count());
        }

        private static void AssertGetActivityEventsNeverCalled(Mock<IPowerBIApiClient> client, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.IsFalse(results.Any());
            client.Verify(x => x.Admin.GetPowerBIActivityEvents(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
