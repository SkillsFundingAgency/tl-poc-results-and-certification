using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    [Authorize(Roles = "Site Administrator")]
    public class TlevelHomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ResultsAndCertificationConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly ILearnerServiceApiClient _learnerServiceApiClient;
        private readonly IPersonalLearningRecordApiClient _personalLearningRecordApiClient;

        protected readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public TlevelHomeController(IHttpContextAccessor contextAccessor, ITokenService tokenService,
            IResultsAndCertificationInternalApiClient internalApiClient,
            ILearnerServiceApiClient learnerServiceApiClient,
            IPersonalLearningRecordApiClient personalLearningRecordApiClient,
            ResultsAndCertificationConfiguration config)
        {
            _httpContextAccessor = contextAccessor;
            _tokenService = tokenService;
            _internalApiClient = internalApiClient;
            _learnerServiceApiClient = learnerServiceApiClient;
            _personalLearningRecordApiClient = personalLearningRecordApiClient;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRegisteredTqProviderInformation()
        {
            // Test learners to fetch data from LRS
            var learnerResult = await _learnerServiceApiClient.VerifyLearnerAsync("5216657648", "Sophie", "Ball", "2000-09-24");

            //var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync("2907424695", "Bryan", "Pale", "1990-07-05");

            //var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync("1037293710", "Amelie", "Armstrong", "1910-08-04");
            
            //var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync("1008780935", "Ayla-Alix", "Afzal", "1986-06-25");

            //var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync("1039662551", "Aidan", "Verity", "1989-05-26");

            //var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync("1008272336", "Abbie-Isla", "Cassidy", "1993-09-12");
            var tqProviderId = 14;
            var providerDetailsModel = await _internalApiClient.GetRegisteredTqProviderInformation(tqProviderId);
            return View("TqProviderDetails", providerDetailsModel);
        }
    }
}