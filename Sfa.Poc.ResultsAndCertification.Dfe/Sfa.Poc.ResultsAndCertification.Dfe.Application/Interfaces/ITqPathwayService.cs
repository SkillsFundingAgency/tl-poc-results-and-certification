using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces
{
    public interface ITqPathwayService
    {
        IQueryable<TqPathway> GetTqPathways();
        Task<TqPathwayDetails> GetTqPathwayDetailsByIdAsync(int pathwayId);
        Task<TqPathwayDetails> GetTqPathwayDetailsByCodeAsync(string code);
    }
}
