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
        public IEnumerable<ValidationError> ValidationErrors
        {
            get
            {
                var result = new List<ValidationError>();
                var invalidReg = Registrations.Where(x => !x.IsValid).ToList();
                invalidReg.ForEach(x => { result.AddRange(x.ValidationErrors); });
                return result;
            }
        }

        public bool IsValid { get { return !Registrations.Any(x => !x.IsValid); } }
    }
}
