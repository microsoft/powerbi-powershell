/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public static class PublishDatasourceToGatewayClusterRequestFactory
    {
        public static PublishDatasourceToGatewayClusterRequest Create<T>(
            DatasourceType                                 datasourceType,
            ConnectionDetails<T>                           connectionDetails,
            string                                         datasourceAnnotation,
            IDictionary<Guid, DatasourceCredentialDetails> credentialDetails,
            string                                         datasourceName,
            MashupTestConnectionDetails                    mashupTestConnectionDetails,
            SingleSignOnType                               singleSignOnType) where T : ConnectionDetails<T>
        {
            return new PublishDatasourceToGatewayClusterRequest
                   {
                        DatasourceType = datasourceType.ToString(),
                        ConnectionDetails = JsonConvert.SerializeObject(connectionDetails),
                        Annotation = datasourceAnnotation,
                        CredentialDetailsDictionary = credentialDetails,
                        DatasourceName = datasourceName,
                        MashupTestConnectionDetails = mashupTestConnectionDetails,
                        SingleSignOnType = singleSignOnType.ToString()
                   };
        }
    }
}