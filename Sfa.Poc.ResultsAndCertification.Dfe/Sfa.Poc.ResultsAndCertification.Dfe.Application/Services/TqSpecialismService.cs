using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Services
{
    public class TqSpecialismService : ITqSpecialismService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TqSpecialism> _specialismRepository;

        public TqSpecialismService(IMapper mapper, IRepository<TqSpecialism> specialismRepository)
        {
            _mapper = mapper;
            _specialismRepository = specialismRepository;
        }

        public IQueryable<TqSpecialism> GetTqSpecialisms()
        {
            return _specialismRepository.GetMany();
        }

        public async Task<TqSpecialismDetails> GetTqSpecialismDetailsByIdAsync(int routeId)
        {
            var route = await _specialismRepository.GetSingleOrDefault(o => o.Id == routeId);
            return _mapper.Map<TqSpecialism, TqSpecialismDetails>(route);
        }

        public async Task<TqSpecialismDetails> GetTqSpecialismDetailsByCodeAsync(string code)
        {
            var route = await _specialismRepository.GetSingleOrDefault(o => o.Code == code);
            return _mapper.Map<TqSpecialism, TqSpecialismDetails>(route);
        }
    }
}
