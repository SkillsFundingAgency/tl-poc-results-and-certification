using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface ITqProviderService
    {
        IQueryable<TqProvider> GetTqProviders();
        Task<IList<TqProviderDetails>> GetTqProvidersAsync();
        Task<TqProviderDetails> GetTqProviderDetailsByIdAsync(int tqProviderId);
        Task<RegisteredTqProviderDetails> GetRegisteredTqProviderDetailsByIdAsync(int tqProviderId);
        Task<int> CreateTqProvidersAsync(TqProviderDetails tqProviderDetails);
        Task<bool> CheckIfTqProviderAlreadyExistsAsync(int awardingOrganiasationId, int providerId, int routeId, int pathwayId, int specialisamId);
        Task<bool> CheckIfTqProviderAlreadyExistsAsync(TqProviderDetails tqProviderDetails);
    }
}
