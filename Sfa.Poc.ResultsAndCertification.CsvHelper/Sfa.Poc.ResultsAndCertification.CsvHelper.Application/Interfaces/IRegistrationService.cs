using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<CoreAndSpecialisms>> GetAllTLevelsByAoUkprnAsync(long ukPrn);
        Task<IEnumerable<Registration>> ValidateRegistrationTlevelsAsync(long ukprn, IEnumerable<Registration> regdata);
        Task<bool> SaveBulkRegistrationsAsync(IEnumerable<Registration> regdata, long ukprn);
        Task ProcessRegistrations(IList<TqRegistration> registrations);
        Task ReadRegistrations(IList<TqRegistration> registrations);
        Task CompareRegistrations();

        Task CompareAndProcessRegistrations();
        IEnumerable<TqRegistrationProfile> TransformRegistrationModel(IList<Registration> stageTwoResponse);
    }
}
