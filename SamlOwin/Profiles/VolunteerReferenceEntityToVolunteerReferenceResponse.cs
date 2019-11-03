using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerReferenceEntityToVolunteerReferenceResponse : Profile
    {
        public VolunteerReferenceEntityToVolunteerReferenceResponse()
        {
            CreateMap<csc_VolunteerReference, VolunteerReferenceResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerReferenceId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.csc_name))
                .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(s => s.csc_ReferenceTypeEnum))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(s => s.csc_Title))
                .ForMember(dest => dest.Occupation, opt => opt.MapFrom(s => s.csc_Employer))
                .ForMember(dest => dest.FirstContactMethod, opt => opt.MapFrom(s => s.csc_FirstContactMethodTypeEnum))
                .ForMember(dest => dest.FirstContactValue, opt => opt.MapFrom(s => s.csc_FirstContactValue))
                .ForMember(dest => dest.SecondContactMethod, opt => opt.MapFrom(s => s.csc_SecondContactMethodTypeEnum))
                .ForMember(dest => dest.SecondContactValue, opt => opt.MapFrom(s => s.csc_SecondContactValue))
                .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(s => s.csc_Volunteer))
                ;
        }
    }
}