
namespace Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models
{
    public class TqSpecialism : BaseEntity
    {
        public int PathwayId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual TqPathway Pathway { get; set; }
    }
}
