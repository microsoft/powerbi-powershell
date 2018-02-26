using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    class PowerBILoggerFactory : IPowerBILoggerFactory
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