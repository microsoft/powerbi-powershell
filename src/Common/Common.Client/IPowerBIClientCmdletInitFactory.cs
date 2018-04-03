/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Client
{
    public interface IPowerBIClientCmdletInitFactory : IPowerBICmdletInitFactory
    {
        IPowerBIClientFactory Client { get; }
    }
}
