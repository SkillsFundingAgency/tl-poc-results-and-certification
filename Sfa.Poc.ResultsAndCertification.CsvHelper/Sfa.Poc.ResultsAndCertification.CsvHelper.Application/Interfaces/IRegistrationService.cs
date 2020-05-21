using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Tlevel>> GetAllTLevelsByAoUkprnAsync(long ukPrn);
        Task<BulkRegistrationResponse> ValidateRegistrationTlevelsAsync(long ukprn, IEnumerable<Registration> regdata);
        Task<bool> SaveBulkRegistrationsAsync(IEnumerable<Registration> regdata, long ukprn);

        Task ProcessRegistrations(IList<TqRegistration> registrations);

        Task ReadRegistrations(IList<TqRegistration> registrations);
    }
}
