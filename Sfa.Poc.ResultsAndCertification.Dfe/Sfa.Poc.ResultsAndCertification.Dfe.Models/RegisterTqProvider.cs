using System.ComponentModel.DataAnnotations;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Models
{
    public class RegisterTqProvider
    {
        [Required]
        public string UkAoCode { get; set; }
        [Required]
        public string UkAoName { get; set; }
        [Required]
        public long UkProviderCode { get; set; }
        [Required]
        public string UkProviderName { get; set; }
        [Required]
        public string TqRouteCode { get; set; }
        [Required]
        public string TqRouteName { get; set; }
        [Required]
        public string TqPathwayCode { get; set; }
        [Required]
        public string TqPathwayName { get; set; }
        [Required]
        public string TqSpecialismCode { get; set; }
        [Required]
        public string TqSpecialismName { get; set; }
    }
}
