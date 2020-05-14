using CsvHelper.Configuration.Attributes;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models
{
    public class Registration
    {
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Uln)]
        public long Uln { get; set; }
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Ukprn)]
        public long Ukprn { get; set; }
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.StartDate)]
        public string StartDate { get; set; } 
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Core)]
        public string Core { get; set; }
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Specialism1)]
        public string Specialism1 { get; set; }
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Specialism2)]
        public string Specialism2 { get; set; }
    }
}
