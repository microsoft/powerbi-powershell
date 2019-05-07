/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Api.Helpers;

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
            return this.Client.Reports.GetReportsAsAdmin(Guid.Empty, filter: filter, top: top, skip: skip).Value?.Select(x => (Report)x);
        }

        public IEnumerable<Report> GetReportsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Reports.GetReportsAsAdmin(groupId: workspaceId, filter: filter, top: top, skip: skip).Value?.Select(x => (Report)x);
        }

        public IEnumerable<Report> GetReportsForWorkspace(Guid workspaceId)
        {
            return this.Client.Reports.GetReports(groupId: workspaceId).Value?.Select(x => (Report)x);
        }

        public Stream ExportReport(Guid reportId, Guid? workspaceId = default)
        {
            return workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Reports.ExportReport(groupId: workspaceId.Value, reportId: reportId) :
                this.Client.Reports.ExportReport(reportId: reportId);
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
            return this.Client.Dashboards.GetDashboards(groupId: workspaceId).Value?.Select(x => (Dashboard)x);
        }

        public IEnumerable<Dashboard> GetDashboardsAsAdminForWorkspace(Guid workspaceId, string filter = default, int? top = default, int? skip = default)
        {
            return this.Client.Dashboards.GetDashboardsAsAdmin(groupId: workspaceId, filter: filter, top: top, skip: skip).Value?.Select(x => (Dashboard)x);
        }

        public IEnumerable<Tile> GetTiles(Guid dashboardId)
        {
            return this.Client.Dashboards.GetTiles(dashboardId: dashboardId).Value?.Select(x => (Tile)x);
        }

        public IEnumerable<Tile> GetTilesAsAdmin(Guid dashboardId)
        {
            return this.Client.Dashboards.GetTilesAsAdmin(dashboardId: dashboardId).Value?.Select(x => (Tile)x);
        }

        public IEnumerable<Tile> GetTilesForWorkspace(Guid workspaceId, Guid dashboardId)
        {
            return this.Client.Dashboards.GetTiles(groupId: workspaceId, dashboardId: dashboardId).Value?.Select(x => (Tile)x);
        }

        public Import GetImport(Guid importId)
        {
            return this.Client.Imports.GetImport(importId: importId);
        }

        public Import GetImportForWorkspace(Guid workspaceId, Guid importId)
        {
            return this.Client.Imports.GetImportInGroup(groupId: workspaceId, importId: importId);
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
            return this.Client.Imports.GetImports(groupId: workspaceId).Value?.Select(x => (Import)x);
        }

        public Guid PostImport(string datasetDisplayName, string filePath, ImportConflictHandlerModeEnum? nameConflict)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var response = this.Client.Imports.PostImportWithFile(
                    fileStream: fileStream,
                    datasetDisplayName: datasetDisplayName,
                    nameConflict: EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.ImportConflictHandlerMode, ImportConflictHandlerModeEnum>(nameConflict)
                );
                return response.Id;
            }
        }

        public Guid PostImportForWorkspace(Guid workspaceId, string datasetDisplayName, string filePath, ImportConflictHandlerModeEnum? nameConflict)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var response = this.Client.Imports.PostImportWithFileInGroup(
                    groupId: workspaceId,
                    fileStream: fileStream,
                    datasetDisplayName: datasetDisplayName,
                    nameConflict: EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.ImportConflictHandlerMode, ImportConflictHandlerModeEnum>(nameConflict)
                );
                return response.Id;
            }
        }

        public Report PostReport(string reportName, string filePath, ImportConflictHandlerModeEnum nameConflict, int timeout)
        {
            var importId = this.PostImport(reportName, filePath, nameConflict);

            Nullable<DateTime> timeoutAt = null;
            if (timeout > 0)
            {
                timeoutAt = DateTime.Now.AddSeconds(timeout);
            }

            Import import = null;
            do
            {
                import = this.GetImport(importId: importId);

                if (import.ImportState != "Succeeded")
                {
                    if (timeoutAt != null && DateTime.Now > timeoutAt)
                    {
                        throw new TimeoutException();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }

            } while (import.ImportState == "Publishing");

            if (import.ImportState != "Succeeded")
            {
                throw new ImportException(importId, reportName, import.ImportState);
            }

            return import.Reports.Single();
        }

        public Report PostReportForWorkspace(Guid workspaceId, string reportName, string filePath, ImportConflictHandlerModeEnum? nameConflict, int timeout)
        {
            var importId = this.PostImportForWorkspace(workspaceId, reportName, filePath, nameConflict);

            Nullable<DateTime> timeoutAt = null;
            if (timeout > 0)
            {
                timeoutAt = DateTime.Now.AddSeconds(timeout);
            }

            Import import = null;
            do
            {
                import = this.GetImportForWorkspace(workspaceId: workspaceId, importId: importId);

                if (import.ImportState != "Succeeded")
                {
                    if (timeoutAt != null && DateTime.Now > timeoutAt)
                    {
                        throw new TimeoutException();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }

            } while (import.ImportState == "Publishing");

            if (import.ImportState != "Succeeded")
            {
                throw new ImportException(importId, reportName, import.ImportState);
            }

            return import.Reports.Single();
        }

         public Report CopyReport(string reportName, Guid? sourceWorkspaceId, Guid sourceReportId, Guid? targetWorkspaceId, Guid? targetDatasetId)
        {
            var requestBody = new CloneReportRequest()
            {
                Name = reportName,
                TargetModelId = targetDatasetId,
                TargetWorkspaceId = targetWorkspaceId
            };

            return sourceWorkspaceId.HasValue ?
                this.Client.Reports.CloneReport(reportId: sourceReportId, requestBody) :
                this.Client.Reports.CloneReport(groupId: sourceWorkspaceId.Value, reportId: sourceReportId, requestBody);
        }

        public Dashboard AddDashboard(string dashboardName, Guid workspaceId)
        {
            var requestBody = new AddDashboardRequest()
            {
                Name = dashboardName
            };

            return workspaceId.Equals(Guid.Empty) ?
                this.Client.Dashboards.AddDashboard(requestBody) :
                this.Client.Dashboards.AddDashboard(groupId: workspaceId, requestBody);
        }

         public Tile CopyTile(Guid workspaceId, Guid dashboardKey, Guid tileKey, Guid targetDashboardId, Guid? targetWorkspaceId, Guid? targetReportId, Guid? targetModelId, PositionConflictAction? positionConflictAction)
        {
            var requestParameters = new CloneTileRequest()
            {
                PositionConflictAction = EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.PositionConflictAction, PositionConflictAction>(positionConflictAction),
                TargetDashboardId = targetDashboardId,
                TargetModelId = targetModelId,
                TargetReportId = targetReportId,
                TargetWorkspaceId = targetWorkspaceId
            };

            return workspaceId.Equals(Guid.Empty) ?
                this.Client.Dashboards.CloneTile(dashboardKey, tileKey, requestParameters) :
                this.Client.Dashboards.CloneTile(groupId: workspaceId, dashboardKey, tileKey, requestParameters);
        }
    }
}
