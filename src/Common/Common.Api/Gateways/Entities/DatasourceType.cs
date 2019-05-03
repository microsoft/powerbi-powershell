/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public enum DatasourceType
    {
        [EnumMember]
        Unknown = -1,

        [EnumMember]
        Sql = 0,

        [EnumMember]
        AnalysisServices = 1,

        [EnumMember]
        SAPHana = 2,

        [EnumMember]
        File = 3,

        [EnumMember]
        Folder = 4,

        [EnumMember]
        Oracle = 5,

        [EnumMember]
        Teradata = 6,

        [EnumMember]
        SharePointList = 7,

        [EnumMember]
        Web = 8,

        [EnumMember]
        OData = 9,

        [EnumMember]
        DB2 = 10,

        [EnumMember]
        MySql = 11,

        [EnumMember]
        PostgreSql = 12,

        [EnumMember]
        Sybase = 13,

        [EnumMember]
        Extension = 14,

        [EnumMember]
        SAPBW = 15,

        [EnumMember]
        AzureTables = 16,

        [EnumMember]
        AzureBlobs = 17,

        [EnumMember]
        Informix = 18,

        [EnumMember]
        ODBC = 19,

        [EnumMember]
        Excel = 20,

        [EnumMember]
        SharePoint = 21,

        [EnumMember]
        PubNub = 22,

        [EnumMember]
        MQ = 23,

        [EnumMember]
        BizTalk = 24,

        [EnumMember]
        GoogleAnalytics = 25,

        [EnumMember]
        CustomHttpApi = 26,

        [EnumMember]
        Exchange = 27,

        [EnumMember]
        Facebook = 28,

        [EnumMember]
        HDInsight = 29,

        [EnumMember]
        AzureMarketplace = 30,

        [EnumMember]
        ActiveDirectory = 31,

        [EnumMember]
        Hdfs = 32,

        [EnumMember]
        SharePointDocLib = 33,

        [EnumMember]
        PowerQueryMashup = 34,

        [EnumMember]
        OleDb = 35,

        [EnumMember]
        AdoDotNet = 36,

        [EnumMember]
        R = 37,

        /// <summary>
        /// Line of Business
        /// </summary>
        [EnumMember]
        LOB = 38,

        [EnumMember]
        Salesforce = 39,

        [EnumMember]
        CustomConnector = 40,

        [EnumMember]
        SAPBWMessageServer = 41,

        [EnumMember]
        AdobeAnalytics = 42,

        [EnumMember]
        Essbase = 43,
    }
}
