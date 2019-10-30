using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerLanguageEntityToVolunteerLanguageResponse : Profile
    {
        public VolunteerLanguageEntityToVolunteerLanguageResponse()
        {
            CreateMap<csc_VolunteerLanguage, VolunteerLanguageResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerLanguageId))
                .ForMember(dest => dest.CanRead, opt => opt.MapFrom(s => s.csc_Speaks))
                .ForMember(dest => dest.CanSpeak, opt => opt.MapFrom(s => s.csc_Reads))
                .ForMember(dest => dest.CanWrite, opt => opt.MapFrom(s => s.csc_Writes))
                .ForMember(dest => dest.WillInterpret, opt => opt.MapFrom(s => s.csc_CanProvidetInterpretationServices))
                .ForMember(dest => dest.WillTranslate, opt => opt.MapFrom(s => s.csc_CanProvideTranslationServices))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(s => s.csc_Language))
                .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(s => s.csc_Volunteer))
                ;
        }
    }
}