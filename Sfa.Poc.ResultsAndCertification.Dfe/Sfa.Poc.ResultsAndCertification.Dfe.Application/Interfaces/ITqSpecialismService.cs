using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces
{
    public interface ITqSpecialismService
    {
        IQueryable<TqSpecialism> GetTqSpecialisms();
        Task<TqSpecialismDetails> GetTqSpecialismDetailsByIdAsync(int specialismId);
        Task<TqSpecialismDetails> GetTqSpecialismDetailsByCodeAsync(string code);
    }
}
