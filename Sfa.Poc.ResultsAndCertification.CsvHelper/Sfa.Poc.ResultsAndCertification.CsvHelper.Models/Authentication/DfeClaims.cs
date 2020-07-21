using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Authentication
{
    public class DfeClaims
    {
        public Guid ServiceId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public string UKPRN { get; set; }
    }
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
