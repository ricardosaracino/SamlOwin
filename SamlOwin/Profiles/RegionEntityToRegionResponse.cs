using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class RegionEntityToRegionResponse: Profile
    {
        public RegionEntityToRegionResponse()
        {
            CreateMap<csc_Region, RegionResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_RegionId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_NameEn))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_NameFr))
                ;
        }
    }
}