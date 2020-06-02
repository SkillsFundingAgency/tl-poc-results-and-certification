using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            //await _registrationService.CompareAndProcessRegistrations();
            
            foreach (var file in Request.Form.Files) // Todo: foreach? 
            {
                // Stage 2 validation
                var validationResponse = await _csvParserService.ValidateAndParseFileAsync(new RegistrationCsvRecord { File = file });

                // Stage 3 validation.
                await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, validationResponse.Where(x => x.IsValid));
                if (validationResponse.Any(x => !x.IsValid))
                {
                    // Merge both Stage2 and Stage3 validations and return.
                    var invalidRegistrations = validationResponse.Where(x => !x.IsValid);
                    
                    response.Registrations = invalidRegistrations;
                    response.ValidationErrors = response.ValidationMessages; // copy

                    // Step: Map data to DB model type.
                    var tqRegistrations = _registrationService.TransformRegistrationModel(validationResponse.Where(x => x.IsValid).ToList());

                    // Step: Process DB operation
                    //var result = await _registrationService.SaveBulkRegistrationsAsync(tqRegistrations, ukprn);

                    return response;
                }
            }

            return response;
        }
    }
}