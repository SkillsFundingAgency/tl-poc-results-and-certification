using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models
{
    public class BulkRegistrationResponse
    {
        public BulkRegistrationResponse()
        {
            Registrations = new List<Registration>();
            ValidationErrors = new List<ValidationError>();
        }

        public IEnumerable<Registration> Registrations { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public List<ValidationError> ValidationMessages
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

        public byte[] ErrorFileBytes { get; set; }
    }
}
