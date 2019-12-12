using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Mappers
{
    public class TqRouteMapper : Profile
    {
        public TqRouteMapper()
        {
            CreateMap<TqRoute, TqRouteDetails>()
                    .ForMember(m => m.Id, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.Code, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.Name, config => config.MapFrom(r => r.Name))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;
        }
    }
}
