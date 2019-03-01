using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerBI.Common.Api.Gateways.Interfaces
{
    public interface IGatewayV2Client
    {
        Task<IEnumerable<GatewayCluster>> GetGatewayClusters();
    }
}
