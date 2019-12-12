using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions;
using System.IO;

namespace Sfa.Poc.ResultsAndCertification.Functions
{
    public static class BlobTriggerFunction
    {
        [FunctionName("BlobTriggerFunction")]
        public static void Run(
            [BlobTrigger("tl-resultsandcertification-files/{name}", Connection = "BlobStorageConnectionString")]Stream myBlob,
            string name,
            [Inject]IDataService dataService,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var response = dataService.GetConfigurationData();

            log.LogInformation($"C# Blob trigger is running in environment {response.Result.EnvironmentName}");
        }
    }
}
