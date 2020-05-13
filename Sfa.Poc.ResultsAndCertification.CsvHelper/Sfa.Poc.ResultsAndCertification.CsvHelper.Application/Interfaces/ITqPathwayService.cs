using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface ITqPathwayService
    {
        IQueryable<TqPathway> GetTqPathways();
        Task<TqPathwayDetails> GetTqPathwayDetailsByIdAsync(int pathwayId);
        Task<TqPathwayDetails> GetTqPathwayDetailsByCodeAsync(string code);
    }
}
