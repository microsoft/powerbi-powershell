/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.PowerBI.Common.Client
{
    public abstract class PowerBIGetCmdlet : PowerBIClientCmdlet
    {
        protected const string AllParameterSetName = "All";

        public PowerBIGetCmdlet(): base() { }

        public PowerBIGetCmdlet(IPowerBIClientCmdletInitFactory init) : base(init) { }

        [Parameter(Mandatory = true, ParameterSetName = AllParameterSetName)]
        public virtual SwitchParameter All { get; set; }

        protected List<T> ExecuteCmdletWithAll<T>(Func<int, int, IEnumerable<T>> GetData)
        {
            var allItems = new List<T>();
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

            return allItems;
        }
    }
}
