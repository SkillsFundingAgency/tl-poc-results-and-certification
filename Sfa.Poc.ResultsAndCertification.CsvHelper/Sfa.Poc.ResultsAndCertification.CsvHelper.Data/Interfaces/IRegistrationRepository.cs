using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces
{
    public interface IRegistrationRepository : IRepository<TqRegistrationProfile>
    {
        Task<IList<TqRegistration>> BulkInsertOrUpdateRegistrations(List<TqRegistration> entities);

        Task<IList<TqRegistrationProfile>> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> entities);
        Task<IList<TqRegistrationProfile>> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> entities, List<TqRegistrationPathway> pathwayEntities, List<TqRegistrationSpecialism> specialismEntities);
    }
}
