using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model
{
    public class Tlevel
    {
        public long ProviderUkprn { get; set; }
        public int TlPathwayId { get; set; }
        public string PathwayName { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialisms { get; set; }
    }
}
