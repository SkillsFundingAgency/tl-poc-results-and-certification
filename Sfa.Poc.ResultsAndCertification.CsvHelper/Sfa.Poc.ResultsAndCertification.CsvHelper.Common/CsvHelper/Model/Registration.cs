using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class Registration: BaseModel
    {
        [Required]
        [Name(Constants.CsvHeaders.Uln)]
        public int Uln { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.LastName)]
        public string LastName { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.DateOfBirth)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.Ukprn)]
        public long Ukprn { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.StartDate)]
        public DateTime StartDate { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.Core)]
        public string Core { get; set; }

        [Required]
        [Name(Constants.CsvHeaders.Specialisms)]
        public IEnumerable<string> Specialisms { get; set; }

        public int TqProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }

        public int RowNum { get; set; }
    }
}
