/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class SqlConnectionDetails : ConnectionDetails<SqlConnectionDetails>
    {
        [DataMember(Name = "server", Order = 0)]
        public string Server { get; set; }

        [DataMember(Name = "database", Order = 10)]
        public string Database { get; set; }

        public override SqlConnectionDetails Normalize()
        {
            return new SqlConnectionDetails { Server = Server.Trim().ToLowerInvariant(), Database = Database.ToLowerInvariant() };
        }
    }
}
