using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces
{
    public interface IResultsAndCertificationInternalApiClient
    {
        Task<RegisteredTqProviderDetails> GetRegisteredTqProviderInformation(int tqProviderId);
    }
}
