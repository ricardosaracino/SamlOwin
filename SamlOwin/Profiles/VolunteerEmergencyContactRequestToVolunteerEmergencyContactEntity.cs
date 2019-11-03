using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEmergencyContactRequestToVolunteerEmergencyContactEntity : Profile
    {
        public VolunteerEmergencyContactRequestToVolunteerEmergencyContactEntity()
        {
            CreateMap<VolunteerEmergencyContactRequest, csc_VolunteerEmergencyContact>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.csc_name, opt => opt.MapFrom(s => s.FullName))
                .ForMember(dest => dest.csc_AddressLine1, opt => opt.MapFrom(s => s.AddressLine1))
                .ForMember(dest => dest.csc_ContactPrimaryTelephone, opt => opt.MapFrom(s => s.PrimaryTelephone))
                .ForMember(dest => dest.csc_SecondaryPhoneType, opt => opt.MapFrom(s => s.SecondaryTelephone))
                ;
        }
    }
}