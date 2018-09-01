/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public class ReportsClient : PowerBIEntityClient, IReportsClient
    {
        public ReportsClient(IPowerBIClient client) : base(client)
        {
        }

        public IEnumerable<Report> GetReports()
        {
            return this.Client.Reports.GetReports().Value?.Select(x => (Report)x);
        }

        public IEnumerable<Report> GetReportsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Reports.GetReportsAsAdmin(filter: filter, top: top, skip: skip).Value?.Select(x => (Report)x);
        }

        public IEnumerable<Report> GetReportsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Reports.GetReportsAsAdmin(groupId: workspaceId.ToString(), filter: filter, top: top, skip: skip).Value?.Select(x => (Report)x);
        }

        public IEnumerable<Report> GetReportsForWorkspace(Guid workspaceId)
        {
            return this.Client.Reports.GetReports(groupId: workspaceId.ToString()).Value?.Select(x => (Report)x);
        }

        public Stream ExportReport(Guid reportId, Guid? workspaceId = default)
        {
            return workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Reports.ExportReport(groupId: workspaceId.Value.ToString(), reportKey: reportId.ToString()) :
                this.Client.Reports.ExportReport(reportKey: reportId.ToString());
        }

        public IEnumerable<Dashboard> GetDashboards()
        {
            return this.Client.Dashboards.GetDashboards().Value?.Select(x => (Dashboard)x);
        }

        public IEnumerable<Dashboard> GetDashboardsAsAdmin(string filter = default, int? top = default, int? skip = default)
        {
            return this.Client.Dashboards.GetDashboardsAsAdmin(filter: filter, top: top, skip: skip).Value?.Select(x => (Dashboard)x);
        }

        public IEnumerable<Dashboard> GetDashboardsForWorkspace(Guid workspaceId)
        {
            return this.Client.Dashboards.GetDashboards(groupId: workspaceId.ToString()).Value?.Select(x => (Dashboard)x);
        }

        public IEnumerable<Dashboard> GetDashboardsAsAdminForWorkspace(Guid workspaceId, string filter = default, int? top = default, int? skip = default)
        {
            return this.Client.Dashboards.GetDashboardsAsAdmin(groupId: workspaceId.ToString(), filter: filter, top: top, skip: skip).Value?.Select(x => (Dashboard)x);
        }

        public IEnumerable<Tile> GetTiles(Guid dashboardId)
        {
            return this.Client.Dashboards.GetTiles(dashboardKey: dashboardId.ToString()).Value?.Select(x => (Tile)x);
        }

        public IEnumerable<Tile> GetTilesAsAdmin(Guid dashboardId)
        {
            return this.Client.Dashboards.GetTilesAsAdmin(dashboardKey: dashboardId.ToString()).Value?.Select(x => (Tile)x);
        }

        public IEnumerable<Tile> GetTilesForWorkspace(Guid workspaceId, Guid dashboardId)
        {
            return this.Client.Dashboards.GetTiles(groupId: workspaceId.ToString(), dashboardKey: dashboardId.ToString()).Value?.Select(x => (Tile)x);
        }

        public Import GetImport(Guid importId)
        {
            return this.Client.Imports.GetImportById(importId: importId.ToString());
        }

        public Import GetImportForWorkspace(Guid workspaceId, Guid importId)
        {
            return this.Client.Imports.GetImportByIdInGroup(groupId: workspaceId.ToString(), importId: importId.ToString());
        }

        public IEnumerable<Import> GetImports()
        {
            return this.Client.Imports.GetImports().Value?.Select(x => (Import)x);
        }

        public IEnumerable<Import> GetImportsAsAdmin(string expand = default, string filter = default, int? top = default, int? skip = default)
        {
            return this.Client.Imports.GetImportsAsAdmin(expand: expand, filter: filter, top: top, skip: skip).Value?.Select(x => (Import)x);
        }

        public IEnumerable<Import> GetImportsForWorkspace(Guid workspaceId)
        {
            return this.Client.Imports.GetImports(groupId: workspaceId.ToString()).Value?.Select(x => (Import)x);
        }

        public Guid PostImport(string datasetDisplayName, string filePath, ImportConflictHandlerModeEnum nameConflict)
        {
            using (var fileStream = new StreamReader(filePath))
            {
                var response = this.Client.Imports.PostImportWithFile(
                    fileStream: fileStream.BaseStream,
                    datasetDisplayName: datasetDisplayName,
                    nameConflict: nameConflict.ToString()
                );
                return Guid.Parse(response.Id);
            }
        }

        public Guid PostImportForWorkspace(Guid workspaceId, string datasetDisplayName, string filePath, ImportConflictHandlerModeEnum nameConflict)
        {
            using (var fileStream = new StreamReader(filePath))
            {
                var response = this.Client.Imports.PostImportWithFileInGroup(
                    groupId: workspaceId.ToString(),
                    fileStream: fileStream.BaseStream,
                    datasetDisplayName: datasetDisplayName,
                    nameConflict: nameConflict.ToString()
                );
                return Guid.Parse(response.Id);
            }
        }

        public Report PostReport(string reportName, string filePath, ImportConflictHandlerModeEnum nameConflict)
        {
            var importId = this.PostImport(reportName, filePath, nameConflict);

            Import import = null;
            do
            {
                import = this.GetImport(importId: importId);
                if (import.ImportState != "Succeeded")
                    System.Threading.Thread.Sleep(500);

            } while (import.ImportState == "Publishing");

            if (import.ImportState != "Succeeded")
                throw new Exception(string.Format("ImportState is '{0}'", import.ImportState));

            return import.Reports.Single();
        }

        public Report PostReportForWorkspace(Guid workspaceId, string reportName, string filePath, ImportConflictHandlerModeEnum nameConflict)
        {
            var id = this.PostImportForWorkspace(workspaceId, reportName, filePath, nameConflict);

            Import import = null;
            do
            {
                import = this.GetImportForWorkspace(workspaceId: workspaceId, importId: id);
                if (import.ImportState != "Succeeded")
                    System.Threading.Thread.Sleep(500);

            } while (import.ImportState == "Publishing");

            if (import.ImportState != "Succeeded")
                throw new Exception(string.Format("ImportState is '{0}'", import.ImportState));

            return import.Reports.Single();
        }
    }
}
