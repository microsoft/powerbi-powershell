using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;

namespace Microsoft.PowerBI.Common.Api.Gateways.Interfaces
{
    public interface IGatewayV2Client
    {
        Task<IEnumerable<GatewayCluster>> GetGatewayClusters();
    }
}
