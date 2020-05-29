using System;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TqRegistrationPathway : BaseEntity
    {
        public TqRegistrationPathway()
        {
            TqRegistrationSpecialism = new HashSet<TqRegistrationSpecialism>();
        }

        public int TqRegistrationId { get; set; }
        public int TqProviderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }

        public virtual TqProvider TqProvider { get; set; }
        public virtual TqRegistrationProfile TqRegistration { get; set; }
        public virtual ICollection<TqRegistrationSpecialism> TqRegistrationSpecialism { get; set; }
    }
}
