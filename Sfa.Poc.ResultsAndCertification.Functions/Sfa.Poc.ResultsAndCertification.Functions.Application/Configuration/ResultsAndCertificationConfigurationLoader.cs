﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace Sfa.Poc.ResultsAndCertification.Functions.Application.Configuration
{
    public class ResultsAndCertificationConfigurationLoader
    {
        public static ResultsAndCertificationConfiguration Load(string environment, string storageConnectionString,
            string version, string serviceName)
        {
            try
            {
                var conn = CloudStorageAccount.Parse(storageConnectionString);
                var tableClient = conn.CreateCloudTableClient();
                var table = tableClient.GetTableReference("Configuration");

                var operation = TableOperation.Retrieve(environment, $"{serviceName}_{version}");
                var result = table.ExecuteAsync(operation).GetAwaiter().GetResult();

                var dynResult = result.Result as DynamicTableEntity;
                var data = dynResult?.Properties["Data"].StringValue;

                return JsonConvert.DeserializeObject<ResultsAndCertificationConfiguration>(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Configuration could not be loaded. Please check your configuration files or see the inner exception for details", ex);
            }
        }
    }
}
