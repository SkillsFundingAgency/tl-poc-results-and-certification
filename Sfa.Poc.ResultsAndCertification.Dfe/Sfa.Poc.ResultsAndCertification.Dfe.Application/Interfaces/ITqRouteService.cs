using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces
{
    public interface ITqRouteService
    {
        IQueryable<TqRoute> GetTqRoutes();
        Task<TqRouteDetails> GetTqRouteDetailsByIdAsync(int routeId);
        Task<TqRouteDetails> GetTqRouteDetailsByCodeAsync(string code);
    }
}
