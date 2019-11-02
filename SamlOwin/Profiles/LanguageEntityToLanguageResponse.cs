using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class LanguageEntityToLanguageResponse : Profile
    {
        public LanguageEntityToLanguageResponse()
        {
            CreateMap<csc_Language, LanguageResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_LanguageId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_LanguageEnglish))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_LanguageFrench))
                ;
        }
    }
}