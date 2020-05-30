using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ICsvHelperService<RegistrationCsvRecord, Registration> _csvParserService;
        private readonly IRegistrationService _registrationService;

        public RegistrationController(ICsvHelperService<RegistrationCsvRecord, Registration> csvParserService, IRegistrationService registrationService)
        {
            _csvParserService = csvParserService;
            _registrationService = registrationService;
        }

        [HttpPost]
        [Route("bulk-upload", Name = "BulkUpload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync()
        {
            long ukprn = 10009696; /*NCFE*/
            var response = new BulkRegistrationResponse();

            //await _registrationService.CompareRegistrations();
            await _registrationService.CompareAndProcessRegistrations();

            foreach (var file in Request.Form.Files)
            {
                var res = await _csvParserService.ValidateAndParseFileAsync(new RegistrationCsvRecord { File = file });
                response.Registrations = await _csvParserService.ReadDataAsync(file);
            }

            // Step: Proceed with validation aginst to DB.
            var validationResult = await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, response.Registrations.Where(x => x.IsValid));
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (response.Registrations.Any(x => !x.IsValid))
                response.ValidationErrors = response.ValidationMessages;

            // var result = await _registrationService.SaveBulkRegistrationsAsync(response.Registrations, ukprn);
            
            return response;
        }
    }
}