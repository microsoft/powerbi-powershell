/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Reflection;

namespace Microsoft.PowerBI.Common.Abstractions.Utilities
{
    public static class DirectoryUtility
    {
        public static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var fileUri = new UriBuilder(codeBase);
            var directory = Uri.UnescapeDataString(fileUri.Path);
            directory = Path.GetDirectoryName(directory);
            if (string.IsNullOrEmpty(fileUri.Host))
            {
                return directory;
            }
            else
            {
                // Running on a fileshare, host from fileUri will be null when executing on a system drive
                // Path.GetDirectoryName will remove the hostname for the file share so it needs to be added back
                directory = $"\\\\{fileUri.Host}\\" + directory.TrimStart(new[] { '\\' });
                return directory;
            }
        }
    }
}
