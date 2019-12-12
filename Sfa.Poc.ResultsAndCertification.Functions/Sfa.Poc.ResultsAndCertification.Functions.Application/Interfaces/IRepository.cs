using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Functions.Application.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<string>> GetStaticData();
    }
}
