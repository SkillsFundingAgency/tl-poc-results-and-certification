﻿using System;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TqRegistrationSpecialism : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }
        public int TlSpecialismId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }

        public virtual TlSpecialism TlSpecialism { get; set; }
        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
    }
}
