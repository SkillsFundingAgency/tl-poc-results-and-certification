
namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public class TqProvider : BaseEntity
    {
        public int AwardingOrganisationId { get; set; }
        public int ProviderId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int SpecialismId { get; set; }

        public virtual TqAwardingOrganisation AwardingOrganisation { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual TqRoute Route { get; set; }
        public virtual TqPathway Pathway { get; set; }
        public virtual TqSpecialism Specialism { get; set; }
    }
}
