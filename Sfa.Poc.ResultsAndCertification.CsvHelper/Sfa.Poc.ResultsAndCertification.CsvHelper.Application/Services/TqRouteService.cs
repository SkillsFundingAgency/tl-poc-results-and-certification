using AutoMapper;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class TqRouteService : ITqRouteService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TqRoute> _routeRepository;

        public TqRouteService(IMapper mapper, IRepository<TqRoute> routeRepository)
        {
            _mapper = mapper;
            _routeRepository = routeRepository;
        }

        public IQueryable<TqRoute> GetTqRoutes()
        {
            return _routeRepository.GetMany();
        }

        public async Task<TqRouteDetails> GetTqRouteDetailsByIdAsync(int routeId)
        {
            var route = await _routeRepository.GetSingleOrDefault(o => o.Id == routeId);
            return _mapper.Map<TqRoute, TqRouteDetails>(route);
        }

        public async Task<TqRouteDetails> GetTqRouteDetailsByCodeAsync(string code)
        {
            var route = await _routeRepository.GetSingleOrDefault(o => o.Code == code);
            return _mapper.Map<TqRoute, TqRouteDetails>(route);
        }        
    }
}
