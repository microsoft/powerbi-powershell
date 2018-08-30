using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Client;
using Microsoft.PowerBI.Common.Api.Reports;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(Report))]
    public class NewPowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.New;
        public const string CmdletName = "PowerBIReport";

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        #endregion
        #region Constructors
        public NewPowerBIReport() : base() { }

        public NewPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        public override void ExecuteCmdlet()
        {

            if (this.Name == null)
            {
                var report = new System.IO.FileInfo(this.Path);
                this.Name = report.Name.Replace(".pbix", "");
            }

            using (var client = this.CreateClient())
            {
                var id = client.Imports.PostImport(this.Name, this.Path);
                var import = client.Reports.GetImport(id);
                var report = import.Reports.Single();

                this.Logger.WriteObject(report, true);
            }
        }
    }
}
