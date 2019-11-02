using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class ProvinceOrStateEntityToProvinceOrStateResponse : Profile
    {
        public ProvinceOrStateEntityToProvinceOrStateResponse()
        {
            CreateMap<csc_ProvinceOrState, ProvinceOrStateResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_ProvinceOrStateId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_ProvinceOrStateEnglish))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_ProvinceOrStateFrench))
                ;
        }
    }
}