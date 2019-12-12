using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.Functions.Extensions;
using Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Functions
{
    public static class HttpTriggerFunction
    {
        [FunctionName("HttpTriggerFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Inject]IDataService dataService,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var response = dataService.GetConfigurationData();

            return response != null
                ? (ActionResult)new OkObjectResult(response.Result)
                : new BadRequestObjectResult("Something went wrong!");
        }
    }
}
