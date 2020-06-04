using Microsoft.AspNetCore.Http;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models
{
    public class BulkRegistrationRequest
    {
        public long Ukprn { get; set; }
        public string performedBy { get; set; }
        public IFormFile RegistrationFile { get; set; }
    }
}
