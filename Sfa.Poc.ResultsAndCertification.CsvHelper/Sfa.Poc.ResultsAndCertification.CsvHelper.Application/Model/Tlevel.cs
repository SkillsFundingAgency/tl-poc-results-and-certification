using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model
{
    public class Tlevel
    {
        public long ProviderUkprn { get; set; }
        public int TlPathwayId { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public IEnumerable<int> TlSpecialisms { get; set; }
        public IEnumerable<string> TlSpecialismLarIds { get; set; }
    }
}
