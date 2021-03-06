﻿using AutoMapper;
using Sfa.Poc.ResultsAndCertification.Dfe.Domain.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Application.Mappers
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<Provider, ProviderDetails>()
                    .ForMember(m => m.Id, config => config.MapFrom(r => r.Id))
                    .ForMember(m => m.Ukprn, config => config.MapFrom(r => r.Ukprn))
                    .ForMember(m => m.Name, config => config.MapFrom(r => r.Name))
                    .ForMember(m => m.DisplayName, config => config.MapFrom(r => r.DisplayName))
                    .ForMember(m => m.IsTLevelProvider, config => config.MapFrom(r => r.IsTLevelProvider))
                    .ForAllOtherMembers(config => config.Ignore())
                    ;
        }
    }
}
