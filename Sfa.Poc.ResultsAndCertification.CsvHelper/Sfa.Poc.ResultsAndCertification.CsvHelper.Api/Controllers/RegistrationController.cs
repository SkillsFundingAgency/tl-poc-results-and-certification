using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ICsvHelperService _csvParserService;
        private readonly IRegistrationService _registrationService;

        public RegistrationController(ICsvHelperService csvParserService, IRegistrationService registrationService)
        {
            _csvParserService = csvParserService;
            _registrationService = registrationService;
        }

        [HttpPost]
        [Route("bulkupload", Name = "BulkUpload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(IFormFile registrationFile)
        {
            var regdata = await _csvParserService.ReadDataAsync(registrationFile);

            long ukPrn = 10009696;
            var aoTlevels = _registrationService.GetAllTLevelsByAoUkprn(ukPrn);

            return new BulkRegistrationResponse { Registrations = regdata };
        }
    }
}