using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models
{
    public class TqRoute : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TqPathway> TqPathways { get; set; }
    }
}
