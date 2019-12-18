using Sfa.Poc.ResultsAndCertification.Functions.Application.Entities;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces
{
    public interface IDataService
    {
        Task<DataResponse> GetConfigurationData();
    }
}
