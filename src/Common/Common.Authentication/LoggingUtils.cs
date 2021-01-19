/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public static class LoggingUtils
    {
        public static void LogMsal(
            LogLevel level,
            string message,
            bool containsPii,
            IPowerBILogger logger)
        {
            Action<string> logWithLevel;
            switch (level)
            {
                case LogLevel.Error:
                case LogLevel.Warning:
                    logWithLevel = logger.WriteWarning;
                    break;
                case LogLevel.Info:
                    logWithLevel = logger.WriteDebug;
                    break;

                case LogLevel.Verbose:
                default:
                    logWithLevel = logger.WriteVerbose;
                    break;
            }

            logWithLevel(message);
        }
    }
}