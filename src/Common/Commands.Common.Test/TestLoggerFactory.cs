/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestLoggerFactory : IPowerBILoggerFactory
    {
        public TestLogger Logger { get; set; }

        public IPowerBILogger CreateLogger(PSCmdlet cmdlet)
        {
            this.Logger = this.Logger ?? new TestLogger()
            {
                Cmdlet = cmdlet
            };

            return this.Logger;
        }
    }
}
