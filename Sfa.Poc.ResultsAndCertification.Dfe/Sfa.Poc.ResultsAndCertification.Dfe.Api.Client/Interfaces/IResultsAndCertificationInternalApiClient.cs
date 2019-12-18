using Sfa.Poc.ResultsAndCertification.Dfe.Models;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Api.Client.Interfaces
{
    public interface IResultsAndCertificationInternalApiClient
    {
        Task<RegisteredTqProviderDetails> GetRegisteredTqProviderInformation(int tqProviderId);
    }
}
