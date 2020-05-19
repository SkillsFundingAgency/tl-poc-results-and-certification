using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TlRoute : BaseEntity
    {
        public TlRoute()
        {
            TlPathways = new HashSet<TlPathway>();
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<TlPathway> TlPathways { get; set; }
    }
}
