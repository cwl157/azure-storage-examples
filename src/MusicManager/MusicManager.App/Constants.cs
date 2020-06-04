using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManager.App
{
    public static class Constants
    {
        // Azure sql
        public static readonly string DbConnectionString = "<azure_sql_connection_string>";

        // Azure Cosmos DB
        public static readonly string CosmosEndpointUri = "cosmos_db_endpoint";
        public static readonly string CosmosPrimaryKey = "cosmos_primary_key";
        public static string CosmosDatabaseId = "cosmos_database_id";
        public static string CosmosContainerId = "cosmos_container_id";

        // Local json file
        public static readonly string LocalFilePath = @"local_file_path_to_json_file";

        // Azure File Storage
        public static readonly string AzureFileConnectionString = "azure_file_connection_string";
        public static readonly string AzureFileShareName = "azure_files_share_name";
        public static readonly string AzureFileFilepath = "azure_file_path_to_json_file";
    }
}
