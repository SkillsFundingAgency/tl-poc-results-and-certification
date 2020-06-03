using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class RegistrationCsvRecord : BaseModel
    {
        [Required]
        [Column(Order = 0)]
        [Name(Constants.CsvHeaders.Uln)]
        public string Uln { get; set; }

        [Required]
        [Column(Order = 1)]
        [Name(Constants.CsvHeaders.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [Column(Order = 2)]
        [Name(Constants.CsvHeaders.LastName)]
        public string LastName { get; set; }

        [Required]
        [Column(Order = 3)]
        [Name(Constants.CsvHeaders.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [Required]
        [Column(Order = 4)]
        [Name(Constants.CsvHeaders.Ukprn)]
        public string Ukprn { get; set; }

        [Required]
        [Column(Order = 5)]
        [Name(Constants.CsvHeaders.StartDate)]
        public string StartDate { get; set; }

        [Required]
        [Column(Order = 6)]
        [Name(Constants.CsvHeaders.Core)]
        public string Core { get; set; }

        [Required]
        [Column(Order = 7)]
        [Name(Constants.CsvHeaders.Specialisms)]
        public string Specialisms { get; set; }

        [Ignore]
        public int RowNum { get; set; }
    }
}
