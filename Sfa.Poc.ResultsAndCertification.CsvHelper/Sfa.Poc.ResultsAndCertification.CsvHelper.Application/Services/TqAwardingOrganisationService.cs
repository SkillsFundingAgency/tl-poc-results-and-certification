using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class TqAwardingOrganisationService : ITqAwardingOrganisationService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TqAwardingOrganisation> _awardingOrganisationRepository;

        public TqAwardingOrganisationService(IMapper mapper, IRepository<TqAwardingOrganisation> awardingOrganisationRepository)
        {
            _mapper = mapper;
            _awardingOrganisationRepository = awardingOrganisationRepository;
        }

        public IQueryable<TqAwardingOrganisation> GetTqAwardingOrganisations()
        {
            return _awardingOrganisationRepository.GetMany();
        }

        public async Task<TqAwardingOrganisationDetails> GetTqAwardingOrganisationDetailsByIdAsync(int awardingOrganisationId)
        {
            var awardingOrganisation = await _awardingOrganisationRepository.GetSingleOrDefault(o => o.Id == awardingOrganisationId);
            return _mapper.Map<TqAwardingOrganisation, TqAwardingOrganisationDetails>(awardingOrganisation);
        }

        public async Task<ServiceResponse<TqAwardingOrganisationDetails>> GetTqAwardingOrganisationDetailsByCodeAsync(string aoCode)
        {
            var awardingOrganisation = await _awardingOrganisationRepository.GetSingleOrDefault(o => o.UkAon == aoCode);
            if (awardingOrganisation == null) return new ServiceResponse<TqAwardingOrganisationDetails>(false, $"AoCode '{aoCode}' does not exist.");
            return new ServiceResponse<TqAwardingOrganisationDetails>(_mapper.Map<TqAwardingOrganisation, TqAwardingOrganisationDetails>(awardingOrganisation));
        }

        public async Task<IList<TqAwardingOrganisationDetails>> GetTqAwardingOrganisationsAsync()
        {
            var awardingOrganisations = await _awardingOrganisationRepository.GetManyAsync();
            return _mapper.Map<IList<TqAwardingOrganisation>, IList<TqAwardingOrganisationDetails>>(awardingOrganisations);
        }
    }
}
