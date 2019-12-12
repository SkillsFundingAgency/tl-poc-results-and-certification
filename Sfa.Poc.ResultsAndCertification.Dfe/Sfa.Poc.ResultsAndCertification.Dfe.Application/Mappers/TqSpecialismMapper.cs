using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Mappers
{
    public class TqSpecialismMapper : Profile
    {
        public TqSpecialismMapper()
        {
            CreateMap<TqSpecialism, TqSpecialismDetails>()
                    .ForMember(m => m.PathwayId, config => config.MapFrom(r => r.PathwayId))
                    .ForMember(m => m.Id, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.Code, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.Name, config => config.MapFrom(r => r.Name))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;
        }
    }
}
