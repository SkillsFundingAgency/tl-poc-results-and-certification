using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Mappers
{
    public class TqProviderMapper : Profile
    {
        public TqProviderMapper()
        {
            CreateMap<TqProvider, TqProviderDetails>()
                    .ForMember(m => m.AwardingOrganisationId, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.ProviderId, config => config.MapFrom(r => r.ProviderId))
                    .ForMember(m => m.RouteId, config => config.MapFrom(r => r.RouteId))
                    .ForMember(m => m.PathwayId, config => config.MapFrom(r => r.PathwayId))
                    .ForMember(m => m.SpecialismId, config => config.MapFrom(r => r.SpecialismId))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;

            CreateMap<TqProviderDetails, TqProvider>()
                    .ForMember(m => m.AwardingOrganisationId, config => config.MapFrom(r => r.AwardingOrganisationId))
                    .ForMember(m => m.ProviderId, config => config.MapFrom(r => r.ProviderId))
                    .ForMember(m => m.RouteId, config => config.MapFrom(r => r.RouteId))
                    .ForMember(m => m.PathwayId, config => config.MapFrom(r => r.PathwayId))
                    .ForMember(m => m.SpecialismId, config => config.MapFrom(r => r.SpecialismId))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;

            CreateMap<TqProvider, RegisteredTqProviderDetails>()
                    .ForMember(m => m.Id, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.UkAoCode, config => config.MapFrom(r => r.AwardingOrganisation.UkAon))
                    .ForMember(m => m.UkAoName, config => config.MapFrom(r => r.AwardingOrganisation.Name))
                    .ForMember(m => m.UkProviderCode, config => config.MapFrom(r => r.Provider.Ukprn))
                    .ForMember(m => m.UkProviderName, config => config.MapFrom(r => r.Provider.Name))
                    .ForMember(m => m.TqRouteCode, config => config.MapFrom(r => r.Route.Code))
                    .ForMember(m => m.TqRouteName, config => config.MapFrom(r => r.Route.Name))
                    .ForMember(m => m.TqPathwayCode, config => config.MapFrom(r => r.Pathway.Code))
                    .ForMember(m => m.TqPathwayName, config => config.MapFrom(r => r.Pathway.Name))
                    .ForMember(m => m.TqSpecialismCode, config => config.MapFrom(r => r.Specialism.Code))
                    .ForMember(m => m.TqSpecialismName, config => config.MapFrom(r => r.Specialism.Name))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;
        }
    }
}
