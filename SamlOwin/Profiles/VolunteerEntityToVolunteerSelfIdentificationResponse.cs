using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEntityToVolunteerSelfIdentificationResponse: Profile
    {
        public VolunteerEntityToVolunteerSelfIdentificationResponse()
        {
            CreateMap<csc_Volunteer, VolunteerSelfIdentificationResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerId))
                .ForMember(dest => dest.AboriginalTypes, opt => opt.MapFrom(s => s.csc_AboriginalPersonEnum))
                .ForMember(dest => dest.CultureTypes, opt => opt.MapFrom(s => s.csc_CultureGroupsEnum))
                .ForMember(dest => dest.DisabilityTypes, opt => opt.MapFrom(s => s.csc_DisabilitiesEnum))
                .ForMember(dest => dest.MinorityTypes, opt => opt.MapFrom(s => s.csc_MinorityGroupsEnum))
                .ForMember(dest => dest.AgreeSelfIdentification, opt => opt.MapFrom(s => s.csc_PermissiontouseSelfIdentification))
                ;
        }
    }
}