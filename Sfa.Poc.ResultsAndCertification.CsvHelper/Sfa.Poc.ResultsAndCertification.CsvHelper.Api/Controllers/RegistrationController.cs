using System.Linq;
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
        [Route("bulk-upload", Name = "BulkUpload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync()
        {
            long ukprn = 1024;
            var response = new BulkRegistrationResponse();
            foreach (var file in Request.Form.Files)
            {
                response.Registrations = await _csvParserService.ReadDataAsync(file);
            }

            if (response.Registrations.Any(x => !x.IsValid))
                return response;

            // Step: Proceed with validation aginst to DB.
            var validationResult = await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, response.Registrations.Where(x => x.IsValid));
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var result = await _registrationService.SaveBulkRegistrationsAsync(response.Registrations, ukprn);
            return response;
        }
    }
}