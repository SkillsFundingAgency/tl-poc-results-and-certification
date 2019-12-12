using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Services
{
    public class TqPathwayService : ITqPathwayService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TqPathway> _pathwayRepository;

        public TqPathwayService(IMapper mapper, IRepository<TqPathway> pathwayRepository)
        {
            _mapper = mapper;
            _pathwayRepository = pathwayRepository;
        }

        public IQueryable<TqPathway> GetTqPathways()
        {
            return _pathwayRepository.GetMany();
        }

        public async Task<TqPathwayDetails> GetTqPathwayDetailsByIdAsync(int pathwayId)
        {
            var pathway = await _pathwayRepository.GetSingleOrDefault(o => o.Id == pathwayId);
            return _mapper.Map<TqPathway, TqPathwayDetails>(pathway);
        }

        public async Task<TqPathwayDetails> GetTqPathwayDetailsByCodeAsync(string code)
        {
            var pathway = await _pathwayRepository.GetSingleOrDefault(o => o.Code == code);
            return _mapper.Map<TqPathway, TqPathwayDetails>(pathway);
        }
    }
}
