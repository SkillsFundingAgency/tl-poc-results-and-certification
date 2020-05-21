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
        [Route("bulkupload", Name = "BulkUpload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(IFormFile registrationFile, long ukprn = 10009696)
        {
            // TODO: Read file from the Blob-storage.
            var regdata = await _csvParserService.ReadDataAsync(registrationFile);

            if (regdata.Any(x => !x.IsValid))
            {
                return new BulkRegistrationResponse
                {
                    Registrations = regdata.Where(x => !x.IsValid)
                };
            }

            // Step: Proceed with validation aginst to DB.
            var validationResult = await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, regdata);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }


            var result = await _registrationService.SaveBulkRegistrationsAsync(regdata, ukprn);

            return new BulkRegistrationResponse { Registrations = regdata };
        }
    }
}