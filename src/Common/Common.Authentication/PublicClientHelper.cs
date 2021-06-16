/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.PowerBI.Common.Authentication
{
    public static class PublicClientHelper
    {
        public static readonly bool IsNetFramework = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework");
    }
}
