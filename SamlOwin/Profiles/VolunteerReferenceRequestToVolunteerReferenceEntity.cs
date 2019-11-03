using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerReferenceRequestToVolunteerReferenceEntity : Profile
    {
        public VolunteerReferenceRequestToVolunteerReferenceEntity()
        {
            CreateMap<VolunteerReferenceRequest, csc_VolunteerReference>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.csc_name, opt => opt.MapFrom(s => s.FullName))
                .ForMember(dest => dest.csc_ReferenceTypeEnum, opt => opt.MapFrom(s => s.ReferenceType))
                .ForMember(dest => dest.csc_Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(dest => dest.csc_Employer, opt => opt.MapFrom(s => s.Occupation))
                .ForMember(dest => dest.csc_FirstContactMethodTypeEnum, opt => opt.MapFrom(s => s.FirstContactMethod))
                .ForMember(dest => dest.csc_FirstContactValue, opt => opt.MapFrom(s => s.FirstContactValue))
                .ForMember(dest => dest.csc_SecondContactMethodTypeEnum, opt => opt.MapFrom(s => s.SecondContactMethod))
                .ForMember(dest => dest.csc_SecondContactValue, opt => opt.MapFrom(s => s.SecondContactValue))
                ;
        }
    }
}