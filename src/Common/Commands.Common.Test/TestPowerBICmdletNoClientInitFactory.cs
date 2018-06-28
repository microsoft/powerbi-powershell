/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestPowerBICmdletNoClientInitFactory : PowerBICmdletInitFactory
    {
        public IPowerBIProfile Profile { get; set; }

        public TestLogger Logger
        {
            get
            {
                if (this.LoggerFactory is TestLoggerFactory logger)
                {
                    return logger.Logger;
                }

                return null;
            }
        }

        public TestPowerBICmdletNoClientInitFactory(bool setProfile, IPowerBIProfile profile = null) :
            base(new TestLoggerFactory(), new ModuleDataStorage(), new TestAuthenticator(), new PowerBISettings())
        {
            if(setProfile)
            {
                this.SetProfile(profile);
            }
        }

        public void SetProfile(IPowerBIProfile profile = null)
        {
            if (profile == null)
            {
                this.Profile = new TestProfile();
            }
            else if (this.Profile != profile)
            {
                this.Profile = profile;
            }

            this.Storage.SetItem("profile", this.Profile);
        }

        public IPowerBIProfile GetProfileFromStorage()
        {
            if (this.Storage.TryGetItem("profile", out IPowerBIProfile profile)) {
                return profile;
            }

            return null;
        }

        public void AssertExpectedUnitTestResults<T>(IEnumerable<T> expected)
        {
            Assert.IsFalse(this.Logger.ErrorRecords.Any());
            var results = this.Logger.Output.ToList();
            Assert.AreEqual(expected.Count(), results.Count());
            var castedResults = results.Cast<T>().ToList();
            CollectionAssert.AreEqual(expected.ToList(), castedResults);
        }
    }
}
