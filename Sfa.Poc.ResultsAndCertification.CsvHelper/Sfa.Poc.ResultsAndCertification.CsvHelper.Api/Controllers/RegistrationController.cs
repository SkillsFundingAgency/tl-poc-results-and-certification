using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
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
            //await _registrationService.CompareAndProcessRegistrations();

            // Stage 2 validation
            IList<Registration> stageTwoResponse = new List<Registration>();
            foreach (var file in Request.Form.Files)
            {
                stageTwoResponse = await _csvParserService.ValidateAndParseFileAsync(new RegistrationCsvRecord { File = file });
            }

            // Stage 3 validation.
            var stageThreeResponse = await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, stageTwoResponse.Where(x => x.IsValid));
            if (stageTwoResponse.Any(x => !x.IsValid) || stageThreeResponse.Any(x => !x.IsValid))
            {
                var invalidRegistrations = stageTwoResponse.Where(x => !x.IsValid)
                                                .Concat(stageThreeResponse.Where(x => !x.IsValid));
                response.Registrations = invalidRegistrations;
                response.ValidationErrors = response.ValidationMessages;
                return response;
            }

            //var result = await _registrationService.SaveBulkRegistrationsAsync(stageTwoResponse, ukprn);
            
            return response;
        }
    }
}