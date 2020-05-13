using System.Linq;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface ITqRouteService
    {
        IQueryable<TqRoute> GetTqRoutes();
        Task<TqRouteDetails> GetTqRouteDetailsByIdAsync(int routeId);
        Task<TqRouteDetails> GetTqRouteDetailsByCodeAsync(string code);
    }
}
