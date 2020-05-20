using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models
{
    public class BulkRegistrationResponse
    {
        public BulkRegistrationResponse()
        {
            Registrations = new List<Registration>();
        }

        public IEnumerable<Registration> Registrations { get; set; }
    }
}
