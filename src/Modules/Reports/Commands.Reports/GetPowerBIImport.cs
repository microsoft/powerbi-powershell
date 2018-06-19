using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Import>))]
    public class GetPowerBIImport : PowerBIClientCmdlet, IUserScope, IUserFilter, IUserId, IUserFirstSkip
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIImport";

        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";

        #region Parameters
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("ImportId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string Filter { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }
        #endregion

        #region Constructors
        public GetPowerBIImport() : base() { }

        public GetPowerBIImport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Individual && !string.IsNullOrEmpty(this.Filter))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Filter)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}", ErrorCategory.InvalidArgument);
            }

            if (this.Scope == PowerBIUserScope.Organization && this.WorkspaceId != default)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.WorkspaceId)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Individual)}", ErrorCategory.InvalidArgument);
            }
        }

        public override void ExecuteCmdlet()
        {
            // TODO add to filter

            IEnumerable<Import> imports = null;
            using (var client = this.CreateClient())
            {
                if (this.WorkspaceId != default)
                {
                    imports = client.Reports.GetImportsForWorkspace(this.WorkspaceId);
                }
                else
                {
                    imports = this.Scope == PowerBIUserScope.Individual ?
                        client.Reports.GetImports() :
                        client.Reports.GetImportsAsAdmin(expand: null, filter: this.Filter, top: this.First, skip: this.Skip);
                }
            }

            // TODO filter result

            this.Logger.WriteObject(imports, true);
        }
    }
}
