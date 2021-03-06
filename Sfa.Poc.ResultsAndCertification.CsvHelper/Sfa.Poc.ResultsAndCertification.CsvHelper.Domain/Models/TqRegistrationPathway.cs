﻿using System;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TqRegistrationPathway : BaseEntity
    {
        public TqRegistrationPathway()
        {
            TqRegistrationSpecialisms = new HashSet<TqRegistrationSpecialism>();
        }

        public int TqRegistrationProfileId { get; set; }
        public int TqProviderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }

        public virtual TqProvider TqProvider { get; set; }
        public virtual TqRegistrationProfile TqRegistrationProfile { get; set; }
        public virtual ICollection<TqRegistrationSpecialism> TqRegistrationSpecialisms { get; set; }
    }
}
