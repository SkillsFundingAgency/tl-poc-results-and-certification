using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface IProviderService
    {
        IQueryable<Provider> GetProviders();
        Task<IList<ProviderDetails>> GetProvidersAsync();
        Task<ProviderDetails> GetProviderDetailsByIdAsync(int providerId);
        Task<ProviderDetails> GetProviderDetailsByCodeAsync(long ukprn);
    }
}
