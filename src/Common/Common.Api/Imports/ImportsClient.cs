using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Imports
{
    public class ImportsClient : PowerBIEntityClient, IImportsClient
    {
        public ImportsClient(IPowerBIClient client) : base(client)
        {
        }

        public Guid PostImport(string datasetDisplayName, string filePath)
        {
            using (var fileStream = new StreamReader(filePath))
            {
                var response = this.Client.Imports.PostImportWithFile(
                    fileStream: fileStream.BaseStream, 
                    datasetDisplayName: datasetDisplayName,
                    nameConflict: ImportConflictHandlerMode.CreateOrOverwrite
                );
                return Guid.Parse( response.Id );
            }
        }
    }
}
