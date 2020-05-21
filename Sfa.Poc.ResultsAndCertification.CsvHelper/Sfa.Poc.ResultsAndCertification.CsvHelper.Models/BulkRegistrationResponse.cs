using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models
{
    public class BulkRegistrationResponse
    {
        public BulkRegistrationResponse()
        {
            Registrations = new List<Registration>();
        }

        public IEnumerable<Registration> Registrations { get; set; }

        public bool IsValid { get { return Registrations.Any(x => !x.IsValid); } }
    }
}
