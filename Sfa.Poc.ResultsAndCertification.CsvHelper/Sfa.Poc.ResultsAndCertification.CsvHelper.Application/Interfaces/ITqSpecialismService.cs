using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface ITqSpecialismService
    {
        IQueryable<TqSpecialism> GetTqSpecialisms();
        Task<TqSpecialismDetails> GetTqSpecialismDetailsByIdAsync(int specialismId);
        Task<TqSpecialismDetails> GetTqSpecialismDetailsByCodeAsync(string code);
    }
}
