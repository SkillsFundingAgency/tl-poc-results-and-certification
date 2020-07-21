using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TlSpecialism : BaseEntity
    {
        public TlSpecialism()
        {
            TqSpecialismRegistrations = new HashSet<TqSpecialismRegistration>();
        }

        public int TlPathwayId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual TlPathway TlPathway { get; set; }

        public virtual ICollection<TqSpecialismRegistration> TqSpecialismRegistrations { get; set; }
    }
}
