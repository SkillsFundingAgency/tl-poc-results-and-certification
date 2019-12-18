using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.Dfe.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.Dfe.Web.Authentication.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sfa.Poc.ResultsAndCertification.Dfe.Api.Client.Interfaces;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Web.Controllers
{
    [Authorize(Roles ="Approver")]
    public class TlevelHomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ResultsAndCertificationConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;

        protected readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        public TlevelHomeController(IHttpContextAccessor contextAccessor, ITokenService tokenService, IResultsAndCertificationInternalApiClient internalApiClient, ResultsAndCertificationConfiguration config)
        {
            _httpContextAccessor = contextAccessor;
            _tokenService = tokenService;
            _internalApiClient = internalApiClient;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRegisteredTqProviderInformation()
        {
            var tqProviderId = 14;
            var providerDetailsModel = await _internalApiClient.GetRegisteredTqProviderInformation(tqProviderId);
            return View("TqProviderDetails", providerDetailsModel);
        }
    }
}