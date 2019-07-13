using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class MashupTestConnectionDetails
    {
        [DataMember(Name = "functionName", Order = 0)]
        public string FunctionName { get; set; }

        [DataMember(Name = "moduleName", Order = 10)]
        public string ModuleName { get; set; }

        [DataMember(Name = "moduleVersion", Order = 20)]
        public string ModuleVersion { get; set; }

        [DataMember(Name = "parameters", Order = 30)]
        public IList<MashupFunctionParameterValues> Parameters { get; set; }
    }
}
