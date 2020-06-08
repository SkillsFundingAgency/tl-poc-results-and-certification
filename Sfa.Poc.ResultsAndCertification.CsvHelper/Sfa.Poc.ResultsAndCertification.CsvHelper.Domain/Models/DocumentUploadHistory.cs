using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class DocumentUploadHistory : BaseEntity
    {
        public DocumentUploadHistory()
        {
        }

        public int TlAwardingOrganisationId { get; set; }
        public string FileName { get; set; }
        public string BlobReference { get; set; }
        public int DocumentType { get; set; }
        public int FileType { get; set; }
        public int Status { get; set; }

        public virtual TlAwardingOrganisation TlAwardingOrganisation { get; set; }
        
    }
}
