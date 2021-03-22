using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces
{
    public interface IPersonalLearningRecordApiClient
    {
        Task<bool> GetLearnerEventsAsync(string uln, string firstName, string lastName, string dateOfBirth);
    }
}
