/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    [TestClass]
    public class PowerBILoggerTests
    {
        [TestMethod]
        public void TestDetectingIsPowerBICmdlet()
        {
            var logger = new PowerBILoggerMock()
            {
                Cmdlet = new NonPowerBICmdlet()
            };

            Assert.IsNull(logger.GetPowerBICmdlet);
        }
 
        [TestMethod]
        public void TestWriteWarningAddsMessage()
        {
            var logger = new PowerBILoggerMock();
            string warning = "This is a warning!";
            logger.WriteWarning(warning);

            Assert.AreEqual(1, logger.WarningMessages.Count);
            Assert.AreEqual(warning, logger.WarningMessages[0]);
        }

        [TestMethod]
        public void TestMainThreadDetection()
        {
            int mainThread = Thread.CurrentThread.ManagedThreadId;
            var cmdlet = new PowerBICmdletMock();
            var logger = new PowerBILoggerMock()
            {
                Cmdlet = cmdlet
            };

            Assert.AreEqual(mainThread, cmdlet.MainThreadId);
            Assert.IsTrue(logger.GetIsMainThread);

            // Using Thread instead of Task to avoid ThreadPool executing on main thread
            var backgroundThread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                logger.WriteVerbose("Test");
            });
            backgroundThread.Start();
            Trace.WriteLine($"Managed Thread ID: {backgroundThread.ManagedThreadId}");
            backgroundThread.Join();
            
            Assert.AreEqual(0, logger.VerboseMessages.Count);
            logger.FlushMessages();
            Assert.AreEqual(1, logger.VerboseMessages.Count);
            Assert.AreEqual("Test", logger.VerboseMessages.Single());
        }
    }

    public class NonPowerBICmdlet : PSCmdlet
    {
        // Nothing, just declaring a non
    }

    public class PowerBILoggerMock : PowerBILogger
    {
        public List<string> DebugMessages => new List<string>();

        public List<ErrorRecord> Errors => new List<ErrorRecord>();

        public List<object> Output => new List<object>();

        public List<string> VerboseMessages = new List<string>();

        public List<string> WarningMessages = new List<string>();

        public List<string> HostMessages = new List<string>();

        public IPowerBICmdlet GetPowerBICmdlet => this.PowerBICmdlet;

        public bool GetIsMainThread => this.IsMainThread;

        public PowerBILoggerMock()
        {
            // Override protected members for mocking
            this.IgnoreCommandRuntime = true;

            this.DebugListener = (d) => this.DebugMessages.Add(d.ToString());
            this.ErrorListener = (e) => this.Errors.Add(e);
            this.OutputListener = (o) => this.Output.Add(o);
            this.HostListener = (h) => this.HostMessages.Add(h.ToString());
            this.VerboseListener = (v) => this.VerboseMessages.Add(v.ToString());
            this.WarningListener = (w) => this.WarningMessages.Add(w.ToString());
        }
    }

    public class PowerBICmdletMock : PowerBICmdlet
    {
        public override void ExecuteCmdlet()
        {
            // Nothing
        }
    }
}