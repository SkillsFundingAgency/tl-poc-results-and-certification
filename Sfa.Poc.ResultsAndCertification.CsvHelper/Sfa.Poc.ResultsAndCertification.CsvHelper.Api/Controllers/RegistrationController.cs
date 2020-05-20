using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ICsvHelperService _csvParserService;

        public RegistrationController(ICsvHelperService csvParserService)
        {
            _csvParserService = csvParserService;
        }

        [HttpPost]
        [Route("bulkupload", Name = "BulkUpload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(IFormFile registrationFile)
        {
            var regdata = await _csvParserService.ReadDataAsync(registrationFile);
            return new BulkRegistrationResponse { Registrations = regdata };
        }
    }
}