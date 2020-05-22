namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models
{
    public partial class TqSpecialismRegistration : BaseEntity
    {
        public int TqRegistrationId { get; set; }
        public int TlSpecialismId { get; set; }
        public int Status { get; set; }

        public virtual TqRegistration TqRegistration { get; set; }
        public virtual TlSpecialism TlSpecialism { get; set; }
    }
}
