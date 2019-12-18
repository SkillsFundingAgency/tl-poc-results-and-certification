using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Poc.ResultsAndCertification.Dfe.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;
using Microsoft.EntityFrameworkCore;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Services
{
    public class TqProviderService : ITqProviderService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TqProvider> _tqProviderRepository;

        public TqProviderService(IMapper mapper, IRepository<TqProvider> tqProviderRepository)
        {
            _mapper = mapper;
            _tqProviderRepository = tqProviderRepository;
        }

        public IQueryable<TqProvider> GetTqProviders()
        {
            return _tqProviderRepository.GetMany();
        }

        public async Task<RegisteredTqProviderDetails> GetRegisteredTqProviderDetailsByIdAsync(int tqProviderId)
        {
            var tqProvider = await _tqProviderRepository.GetSingleOrDefault(o => o.Id == tqProviderId,
                                                                            o => o.Provider,
                                                                            o => o.AwardingOrganisation,
                                                                            o => o.Route,
                                                                            o => o.Pathway,
                                                                            o => o.Specialism);
            return _mapper.Map<TqProvider, RegisteredTqProviderDetails>(tqProvider);
        }

        public async Task<TqProviderDetails> GetTqProviderDetailsByIdAsync(int tqProviderId)
        {
            var tqProvider = await _tqProviderRepository.GetSingleOrDefault(o => o.Id == tqProviderId);
            return _mapper.Map<TqProvider, TqProviderDetails>(tqProvider);
        }

        public async Task<IList<TqProviderDetails>> GetTqProvidersAsync()
        {
            var tqProviders = await _tqProviderRepository.GetManyAsync();
            return _mapper.Map<IList<TqProvider>, IList<TqProviderDetails>>(tqProviders);
        }

        public async Task<int> CreateTqProvidersAsync(TqProviderDetails tqProviderDetails)
        {
            tqProviderDetails.Id = 0;
            var tqProvider = _mapper.Map<TqProvider>(tqProviderDetails);
            return await _tqProviderRepository.Create(tqProvider);
        }

        public async Task<bool> CheckIfTqProviderAlreadyExistsAsync(int awardingOrganiasationId, int providerId, int routeId, int pathwayId, int specialisamId)
        {
            return await _tqProviderRepository.GetMany(o => o.AwardingOrganisationId == awardingOrganiasationId
                                                                            && o.ProviderId == providerId
                                                                            && o.RouteId == routeId
                                                                            && o.PathwayId == pathwayId
                                                                            && o.SpecialismId == specialisamId).AnyAsync();
        }

        public async Task<bool> CheckIfTqProviderAlreadyExistsAsync(TqProviderDetails tqProviderDetails)
        {
            return await _tqProviderRepository.GetMany(o => o.AwardingOrganisationId == tqProviderDetails.AwardingOrganisationId
                                                                            && o.ProviderId == tqProviderDetails.ProviderId
                                                                            && o.RouteId == tqProviderDetails.RouteId
                                                                            && o.PathwayId == tqProviderDetails.PathwayId
                                                                            && o.SpecialismId == tqProviderDetails.SpecialismId).AnyAsync();
        }
    }
}
