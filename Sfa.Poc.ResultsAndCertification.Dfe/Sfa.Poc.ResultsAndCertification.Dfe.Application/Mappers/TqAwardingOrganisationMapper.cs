using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Mappers
{
    public class TqAwardingOrganisationMapper : Profile
    {
        public TqAwardingOrganisationMapper()
        {
            CreateMap<TqAwardingOrganisation, TqAwardingOrganisationDetails>()
                    .ForMember(m => m.Id, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.UkAon, config => config.MapFrom(r => r.UkAon))
                    .ForMember(m => m.Name, config => config.MapFrom(r => r.Name))
                    .ForMember(m => m.DisplayName, config => config.MapFrom(r => r.DisplayName))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;
        }
    }
}
