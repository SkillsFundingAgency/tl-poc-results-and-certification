using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface IRegistrationService
    {
        IEnumerable<Tlevel> GetAllTLevelsByAoUkprn(long ukPrn);
        Task ProcessRegistrations(IList<TqRegistration> registrations);

        Task ReadRegistrations(IList<TqRegistration> registrations);
    }
}
