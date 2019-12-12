using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Provider> _providerRepository;

        public ProviderService(IMapper mapper, IRepository<Provider> providerRepository)
        {
            _mapper = mapper;
            _providerRepository = providerRepository;
        }

        public IQueryable<Provider> GetProviders()
        {
            return _providerRepository.GetMany();
        }

        public async Task<IList<ProviderDetails>> GetProvidersAsync()
        {
            var awardingOrganisations = await _providerRepository.GetManyAsync();
            return _mapper.Map<IList<Provider>, IList<ProviderDetails>>(awardingOrganisations);
        }

        public async Task<ProviderDetails> GetProviderDetailsByIdAsync(int providerId)
        {
            var provider = await _providerRepository.GetSingleOrDefault(o => o.Id == providerId);
            return _mapper.Map<Provider, ProviderDetails>(provider);
        }

        public async Task<ProviderDetails> GetProviderDetailsByCodeAsync(long ukprn)
        {
            var provider = await _providerRepository.GetSingleOrDefault(o => o.Ukprn == ukprn);
            return _mapper.Map<Provider, ProviderDetails>(provider);
        }        
    }
}
