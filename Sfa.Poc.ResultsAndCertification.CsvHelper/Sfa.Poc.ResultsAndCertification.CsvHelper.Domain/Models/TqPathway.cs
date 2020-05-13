using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public class TqPathway : BaseEntity
    {
        public int RouteId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual TqRoute Route { get; set; }
        public virtual ICollection<TqSpecialism> TqSpecialisms { get; set; }
    }
}
