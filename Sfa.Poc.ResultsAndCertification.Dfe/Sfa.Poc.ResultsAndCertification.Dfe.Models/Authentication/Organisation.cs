using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Models.Authentication
{
    public class Organisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? URN { get; set; }
        public int? UID { get; set; }
        public int? UKPRN { get; set; }
        public int? EstablishmentNumber { get; set; }
        public string Telephone { get; set; }
        public int? LegacyId { get; set; }
        public int? CompanyRegistrationNumber { get; set; }
    }
}
