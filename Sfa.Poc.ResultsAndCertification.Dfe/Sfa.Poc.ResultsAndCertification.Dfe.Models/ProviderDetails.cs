
namespace Sfa.Poc.ResultsAndCertification.Dfe.Models
{
    public class ProviderDetails
    {
        public int Id { get; set; }
        public long Ukprn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsTLevelProvider { get; set; }
    }
}
