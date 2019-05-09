/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Linq;
using Microsoft.PowerBI.Common.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public static class TestUtilities
    {
        static Random randomGenerator = new Random();

        public static void AssertNoCmdletErrors(System.Management.Automation.PowerShell ps)
        {
            Assert.IsFalse(ps.HadErrors, $"Error messages: {string.Join(Environment.NewLine, ps.Streams.Error?.Select(e => e.ToString()))}");
        }

        public static void AssertExpectedUnitTestResults(object expectedResponse, Mock<IPowerBIApiClient> client, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            var response = results.Cast<object>().FirstOrDefault();
            Assert.AreEqual(expectedResponse, response);
            client.VerifyAll();
        }

        public static void AssertExpectedNoOutputForUnitTestResults(Mock<IPowerBIApiClient> client, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output?.ToList();
            if(results != null)
            {
                Assert.AreEqual(0, results.Count, "No output results were expected");
            }

            client.VerifyAll();
        }

        public static string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

        public static string GetRandomString(int length)
        {
            byte[] randomBytes = new byte[randomGenerator.Next(length)];
            randomGenerator.NextBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
