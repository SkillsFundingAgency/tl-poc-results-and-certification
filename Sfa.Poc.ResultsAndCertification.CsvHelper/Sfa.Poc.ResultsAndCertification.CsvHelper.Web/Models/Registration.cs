using CsvHelper.Configuration.Attributes;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models
{
    public class Registration : ValidationState
    {
        [Required]
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Uln)]
        public long Uln { get; set; }
        
        [Required]
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Ukprn)]
        public long Ukprn { get; set; }

        [Required]
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.StartDate)]
        public DateTime StartDate { get; set; }

        [Required]
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Core)]
        public string Core { get; set; }
        
        [Required]
        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Specialism1)]
        public string Specialism1 { get; set; }

        [Name(Utilities.CsvHelper.Constants.CsvHeaders.Specialism2)]
        public string Specialism2 { get; set; }
    }
}
