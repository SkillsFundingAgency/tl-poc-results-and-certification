using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ICsvHelperService<RegistrationCsvRecord, Registration> _csvParserService;
        private readonly IRegistrationService _registrationService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public RegistrationController(ResultsAndCertificationConfiguration configuration, ICsvHelperService<RegistrationCsvRecord, Registration> csvParserService, IRegistrationService registrationService, IBlobStorageService blobStorageService)
        {
            _configuration = configuration;
            _csvParserService = csvParserService;
            _registrationService = registrationService;
            _blobStorageService = blobStorageService;
        }

        [HttpPost]
        [Route("bulk-upload1", Name = "BulkUpload1")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            long ukprn = 10009696; /*NCFE*/
            var performedBy = "system@digtal.com";  // Todo: pass-through model or azure-blob

            var response = new BulkRegistrationResponse();

            //await _registrationService.CompareRegistrations();
            //await _registrationService.CompareAndProcessRegistrations();

            using (var fileStream = await _blobStorageService.DownloadFileAsync(_configuration.BlobStorageConnectionString, "registrations", $"{request.Ukprn}/processing/{request.BlobReferencePath}"))
            {
                if (fileStream == null) return response;
                
                // Stage 2 validation
                var validationResponse = await _csvParserService.ValidateAndParseFileAsync(new RegistrationCsvRecord { FileStream = fileStream });

                if (validationResponse.Any(x => x.IsFileReadDirty))
                {
                    response.ValidationErrors = validationResponse
                                                    .Where(x => x.IsFileReadDirty)
                                                    .Select(x => x.ValidationErrors)
                                                    .FirstOrDefault().ToList();

                    // We are here when unexpected error while reading file, so filedirty is set(above) and add if any half-way row-levels errors(below)
                    // Todo: force test is required or unit-test must check this. 
                    response.Registrations = validationResponse.Where(x => !x.IsValid && !x.IsFileReadDirty);
                    response.ValidationErrors.AddRange(response.ValidationMessages);

                    return response;
                }

                // Stage 3 validation.
                await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, validationResponse.Where(x => x.IsValid));
                if (validationResponse.Any(x => !x.IsValid))
                {
                    // Merge both Stage2 and Stage3 validations and return.
                    var invalidRegistrations = validationResponse.Where(x => !x.IsValid);

                    response.Registrations = invalidRegistrations;
                    response.ValidationErrors = response.ValidationMessages; // copy

                    response.ErrorFileBytes = await _csvParserService.WriteErrorFile(response.ValidationErrors);

                    return response;
                }

                // Step: Map data to DB model type.
                var tqRegistrations = _registrationService.TransformRegistrationModel(validationResponse, performedBy).ToList();

                // Step: Process DB operation
                var result = await _registrationService.CompareAndProcessRegistrations(tqRegistrations);
            }

            return response;
        }


        [HttpPost]
        [Route("bulk-upload", Name = "BulkUpload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync()
        {
            long ukprn = 10009696; /*NCFE*/
            var performedBy = "system@digtal.com";  // Todo: pass-through model or azure-blob

            var response = new BulkRegistrationResponse();

            //await _registrationService.CompareRegistrations();
            //await _registrationService.CompareAndProcessRegistrations();

            foreach (var file in Request.Form.Files) // Todo: foreach? 
            {
                // Stage 2 validation
                var validationResponse = await _csvParserService.ValidateAndParseFileAsync(new RegistrationCsvRecord { File = file });

                if (validationResponse.Any(x => x.IsFileReadDirty))
                {
                    response.ValidationErrors = validationResponse
                                                    .Where(x => x.IsFileReadDirty)
                                                    .Select(x => x.ValidationErrors)
                                                    .FirstOrDefault().ToList();

                    // We are here when unexpected error while reading file, so filedirty is set(above) and add if any half-way row-levels errors(below)
                    // Todo: force test is required or unit-test must check this. 
                    response.Registrations = validationResponse.Where(x => !x.IsValid && !x.IsFileReadDirty);
                    response.ValidationErrors.AddRange(response.ValidationMessages);

                    return response;
                }

                // Stage 3 validation.
                await _registrationService.ValidateRegistrationTlevelsAsync(ukprn, validationResponse.Where(x => x.IsValid));
                if (validationResponse.Any(x => !x.IsValid))
                {
                    // Merge both Stage2 and Stage3 validations and return.
                    var invalidRegistrations = validationResponse.Where(x => !x.IsValid);

                    response.Registrations = invalidRegistrations;
                    response.ValidationErrors = response.ValidationMessages; // copy

                    response.ErrorFileBytes = await _csvParserService.WriteErrorFile(response.ValidationErrors);

                    return response;
                }

                // Step: Map data to DB model type.
                var tqRegistrations = _registrationService.TransformRegistrationModel(validationResponse, performedBy).ToList();

                // Step: Process DB operation
                var result = await _registrationService.CompareAndProcessRegistrations(tqRegistrations);
            }

            return response;
        }
    }
}