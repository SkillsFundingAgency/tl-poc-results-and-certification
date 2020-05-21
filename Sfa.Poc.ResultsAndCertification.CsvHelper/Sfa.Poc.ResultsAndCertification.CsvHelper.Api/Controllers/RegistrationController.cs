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
            var response = new BulkRegistrationResponse();
            foreach (var file in Request.Form.Files)
            {
                response.Registrations = await _csvParserService.ReadDataAsync(file);
            }

            //await _registrationService.ProcessRegistrations(null);
            //await _registrationService.ReadRegistrations(null);

            //long ukPrn = 1024;
            //var aoTlevels = _registrationService.GetAllTLevelsByAoUkprn(ukPrn);
            //await _registrationService.ProcessRegistrations(null);
            return response;
        }
    }
}