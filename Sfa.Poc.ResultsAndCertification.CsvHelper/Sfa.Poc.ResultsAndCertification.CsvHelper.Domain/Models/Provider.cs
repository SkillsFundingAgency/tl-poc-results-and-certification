
namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public class Provider : BaseEntity
    {
        public long Ukprn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsTLevelProvider { get; set; }
    }
}
