using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class Registration : ValidationState
    {
        [Required]
        [Name(Constants.CsvHeaders.Uln)]
        public long Uln { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.Ukprn)]
        public long Ukprn { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.StartDate)]
        public string StartDate { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.Core)]
        public string Core { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.Specialism1)]
        public string Specialism1 { get; set; }

        [Name(Constants.CsvHeaders.Specialism2)]
        public string Specialism2 { get; set; }

        public int RowNum { get; set; }
    }
}
