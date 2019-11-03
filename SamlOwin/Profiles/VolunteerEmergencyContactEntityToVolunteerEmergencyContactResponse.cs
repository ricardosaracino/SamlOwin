using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEmergencyContactEntityToVolunteerEmergencyContactResponse : Profile
    {
        public VolunteerEmergencyContactEntityToVolunteerEmergencyContactResponse()
        {
            CreateMap<csc_VolunteerEmergencyContact, VolunteerEmergencyContactResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerEmergencyContactId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.csc_name))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(s => s.csc_AddressLine1))
                .ForMember(dest => dest.PrimaryTelephone, opt => opt.MapFrom(s => s.csc_ContactPrimaryTelephone))
                .ForMember(dest => dest.SecondaryTelephone, opt => opt.MapFrom(s => s.csc_SecondaryPhoneType))
                .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(s => s.csc_Volunteer))
                ;
        }
    }
}