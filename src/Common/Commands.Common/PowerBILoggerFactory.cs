/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    public class PowerBILoggerFactory : IPowerBILoggerFactory
    {
        public IPowerBILogger CreateLogger(PSCmdlet cmdlet)
        {
            return new PowerBILogger()
            {
                Cmdlet = cmdlet
            };
        }
    }
}