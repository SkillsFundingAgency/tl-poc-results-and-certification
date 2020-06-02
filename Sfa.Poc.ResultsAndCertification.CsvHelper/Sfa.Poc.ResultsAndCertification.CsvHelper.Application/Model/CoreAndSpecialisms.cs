using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model
{
    public class CoreAndSpecialisms
    {

        public CoreAndSpecialisms()
        {
            TlSpecialisms = new List<int>();
            TlSpecialismLarIds = new List<KeyValuePair<int, string>>();
        }

        public long ProviderUkprn { get; set; }
        public int TlPathwayId { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public int TqProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public IEnumerable<int> TlSpecialisms { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }
    }
}
