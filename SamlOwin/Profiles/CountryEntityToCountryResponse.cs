using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class CountryEntityToCountryResponse : Profile
    {
        public CountryEntityToCountryResponse()
        {
            CreateMap<csc_Country, CountryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_CountryId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_CountryEnglish))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_CountryFrench))
                ;
        }
    }
}