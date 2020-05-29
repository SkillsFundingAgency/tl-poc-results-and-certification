using System;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TqRegistrationProfile : BaseEntity
    {
        public TqRegistrationProfile()
        {
            TqRegistrationPathway = new HashSet<TqRegistrationPathway>();
        }

        public int Id { get; set; }
        public int UniqueLearnerNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateofBirth { get; set; }

        public virtual ICollection<TqRegistrationPathway> TqRegistrationPathway { get; set; }
    }
}
