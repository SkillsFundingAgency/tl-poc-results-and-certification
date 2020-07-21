using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces
{
    public interface IResultsAndCertificationInternalApiClient
    {
        Task<RegisteredTqProviderDetails> GetRegisteredTqProviderInformation(int tqProviderId);

        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(IFormFile registrationFile);

        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request);
        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync1(BulkRegistrationRequest request);
    }
}
