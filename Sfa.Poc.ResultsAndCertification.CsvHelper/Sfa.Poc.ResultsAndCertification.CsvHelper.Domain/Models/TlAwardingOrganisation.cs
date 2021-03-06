﻿using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TlAwardingOrganisation : BaseEntity
    {
        public TlAwardingOrganisation()
        {
            DocumentUploadHistorys = new HashSet<DocumentUploadHistory>();
        }

        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<TqAwardingOrganisation> TqAwardingOrganisations { get; set; }
        public virtual ICollection<DocumentUploadHistory> DocumentUploadHistorys { get; set; }
    }
}
