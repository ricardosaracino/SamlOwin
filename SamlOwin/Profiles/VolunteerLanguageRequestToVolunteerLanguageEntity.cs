using AutoMapper;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerLanguageRequestToVolunteerLanguageEntity : Profile
    {
        public VolunteerLanguageRequestToVolunteerLanguageEntity()
        {
            CreateMap<VolunteerLanguageRequest, csc_VolunteerLanguage>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.csc_Speaks, opt => opt.MapFrom(s => s.CanRead))
                .ForMember(dest => dest.csc_Reads, opt => opt.MapFrom(s => s.CanSpeak))
                .ForMember(dest => dest.csc_Writes, opt => opt.MapFrom(s => s.CanWrite))
                .ForMember(dest => dest.csc_CanProvidetInterpretationServices, opt => opt.MapFrom(s => s.WillInterpret))
                .ForMember(dest => dest.csc_CanProvideTranslationServices, opt => opt.MapFrom(s => s.WillTranslate))
                .ForMember(dest => dest.csc_Language,
                    opt => opt.MapFrom(s => new EntityReference(csc_Language.EntityLogicalName, s.Language.Id)))
                ;
        }
    }
}