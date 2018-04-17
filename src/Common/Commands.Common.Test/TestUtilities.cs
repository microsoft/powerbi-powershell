/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public static class TestUtilities
    {
        static Random randomGenerator = new Random();

        public static void AssertNoCmdletErrors(System.Management.Automation.PowerShell ps)
        {
            Assert.IsFalse(ps.HadErrors, $"Error messages: {string.Join(Environment.NewLine, ps.Streams.Error?.Select(e => e.ToString()))}");
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
