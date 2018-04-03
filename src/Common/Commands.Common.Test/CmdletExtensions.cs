/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public static class CmdletExtensions
    {
        public static void InvokeBeginProcessing(this PSCmdlet cmdlet)
        {
            var beginProcessingMethod = typeof(PSCmdlet).GetMethod("BeginProcessing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            beginProcessingMethod.Invoke(cmdlet, null);
        }

        public static void InvokeEndProcessing(this PSCmdlet cmdlet)
        {
            var endProcessingMethod = typeof(PSCmdlet).GetMethod("EndProcessing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            endProcessingMethod.Invoke(cmdlet, null);
        }

        public static void InvokePowerBICmdlet(this PowerBICmdlet cmdlet)
        {
            cmdlet.InvokeBeginProcessing();
            cmdlet.ExecuteCmdlet();
            cmdlet.InvokeEndProcessing();
        }
    }
}
