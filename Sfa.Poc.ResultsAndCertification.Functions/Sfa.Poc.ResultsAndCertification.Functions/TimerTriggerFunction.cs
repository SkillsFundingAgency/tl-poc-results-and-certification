using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions;
using System;

namespace Sfa.Poc.ResultsAndCertification.Functions
{
    public static class TimerTriggerFunction
    {
        [FunctionName("TimerTriggerFunction")]
        public static void Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [Inject]IDataService testService,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var response = testService.GetConfigurationData();
            log.LogInformation($"C# Timer trigger is running in environment {response.Result.EnvironmentName}");
        }
    }
}
