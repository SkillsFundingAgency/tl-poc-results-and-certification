using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces
{
    public interface ILearnerServiceApiClient
    {
        Task<bool> VerifyLearnerAsync(string uln, string firstName, string lastName, string dateOfBirth);
    }
}
