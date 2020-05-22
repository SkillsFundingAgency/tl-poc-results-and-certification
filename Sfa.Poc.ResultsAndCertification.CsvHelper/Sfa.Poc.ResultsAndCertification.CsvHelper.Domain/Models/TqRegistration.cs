using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TqRegistration : BaseEntity
    {
        public TqRegistration()
        {
            TqSpecialismRegistrations = new HashSet<TqSpecialismRegistration>();
        }

        public long UniqueLearnerNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime?  DateofBirth { get; set; }
        public int TqProviderId { get; set; }
        public DateTime StartDate { get; set; }
        public int Status { get; set; }

        public virtual TqProvider TqProvider { get; set; }
        public virtual ICollection<TqSpecialismRegistration> TqSpecialismRegistrations { get; set; }
    }
}
