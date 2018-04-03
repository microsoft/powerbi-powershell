/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Profile;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Api.V2.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class GetPowerBIWorkspaceTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForOrganization()
        {
            /*
             * Test requires a preview Workspace (v2) to exist and login as an administrator
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameter(nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization));
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                if(result.Count == 0)
                {
                    Assert.Inconclusive("No workspaces returned, verify you have workspaces in your organization");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForIndividual()
        {
            /*
             * Test requires at least one workspace (group or preview Workspace)
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                if (result.Count == 0)
                {
                    Assert.Inconclusive("No workspaces returned, verify you are assigned or own any workspaces");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForOrganizationAndFilter()
        {
            /*
             * Test requires a preview Workspace (v2) to exist and login as an administrator.
             * The workspace needs to be named "Preview Workspace" and no workspace exists with the name "Nonexistant Workspace".
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization) },
                        { nameof(GetPowerBIWorkspace.Filter), "name eq 'Preview Workspace'" }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.Filter)] = "name eq 'Nonexistant Workspace'";
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForOrganizationAndUser()
        {
            /*
             * Test requires a preview Workspace (v2) to exist and login as an administrator.
             * The workspace needs to assigned to user User1@granularcontrols1.ccsctp.net and no workspace should be assigned to fakeuser@granularcontrols1.ccsctp.net
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization) },
                        { nameof(GetPowerBIWorkspace.User), "User1@granularcontrols1.ccsctp.net" }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.User)] = "fakeuser@granularcontrols1.ccsctp.net";
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallGetWorkspacesWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo);
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void GetWorkspacesForIndividual()
        {
            // Arrange\Setup
            //var loggerFactory = new TestLoggerFactory();
            //var storage = new ModuleDataStorage();
            //var profile = new TestProfile();
            //var authenticator = new TestAuthenticator();
            //storage.SetItem("profile", profile);

            var group = new Group(id: Guid.NewGuid().ToString(), name: "TestGroup", isReadOnly: false, isOnDedicatedCapacity: false, capacityId: null, description: "Test", type: "Workspace", state: "Active"); // users
            var groupList = new ODataResponseListGroup(value: new List<Group>(new []{ group }));
            var clientHandler = new FakeHttpClientHandler(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(groupList))
            });

            //var testClient = new TestClient(clientHandler);

            //var cmdlet = new GetPowerBIWorkspace(new PowerBIClientCmdletInitFactory(loggerFactory, storage, authenticator, new PowerBISettings(), testClient));
            var initFactory = new TestPowerBICmdletInitFactory(clientHandler);
            initFactory.SetProfile();
            var cmdlet = new GetPowerBIWorkspace(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Verify
            Assert.AreEqual(0, initFactory.Logger.ErrorRecords.Count());
            var results = initFactory.Logger.Output.ToList();
            Assert.AreEqual(1, results.Count());
            var returnedGroup = results.Cast<Group>().First();
            Assert.AreEqual(group.Id, returnedGroup.Id);
            Assert.AreEqual(group.Name, returnedGroup.Name);
            Assert.AreEqual(group.IsReadOnly, returnedGroup.IsReadOnly);
            Assert.AreEqual(group.Description, returnedGroup.Description);
            Assert.AreEqual(group.CapacityId, returnedGroup.CapacityId);
            Assert.AreEqual(group.IsOnDedicatedCapacity, returnedGroup.IsOnDedicatedCapacity);
            Assert.AreEqual(group.State, returnedGroup.State);
            Assert.AreEqual(group.Type, returnedGroup.Type);
            //Assert.AreEqual(group.Users, returnedGroup.Users);
        }
    }
}
