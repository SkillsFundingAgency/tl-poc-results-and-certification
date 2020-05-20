using System;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public class NotificationTemplate : BaseEntity
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }        
    }
}
