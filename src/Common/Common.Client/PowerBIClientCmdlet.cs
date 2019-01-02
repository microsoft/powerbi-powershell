/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Api;

namespace Microsoft.PowerBI.Common.Client
{
    public abstract class PowerBIClientCmdlet : PowerBICmdlet
    {
        protected IPowerBIClientFactory ClientFactory { get; set; }

        public PowerBIClientCmdlet() : this(GetDefaultClientInitFactory())
        {
        }

        protected static IPowerBIClientCmdletInitFactory GetDefaultClientInitFactory() => new PowerBIClientCmdletInitFactory(new PowerBILoggerFactory(), new ModuleDataStorage(), new AuthenticationFactorySelector(), new PowerBISettings(), new PowerBIClientFactory());

        public PowerBIClientCmdlet(IPowerBIClientCmdletInitFactory init) : base(init) => this.ClientFactory = init.Client;

        protected virtual IPowerBIApiClient CreateClient()
        {
            return this.ClientFactory.CreateClient(this.Authenticator, this.Profile, this.Logger, this.Settings);
        }

        protected List<T> ExecuteCmdletWithAll<T>(Func<int, int, IEnumerable<T>> GetData)
        {
            var allItems = new List<T>();
            using (var client = this.CreateClient())
            {
                var top = 5000;
                var skip = 0;
                var count = 0L;

                // API is called with 5000 as the top which retrieves 5000 items in each call.
                // Loop will terminate when the items retrieved are less than 5000.
                do
                {
                    var result = GetData.Invoke(top, skip);
                    allItems.AddRange(result);
                    count = result.Count();
                    skip += top;
                } while (count == top);
            }

            return allItems;
        }
    }
}
