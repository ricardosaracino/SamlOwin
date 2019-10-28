using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEntityToVolunteerModel : Profile
    {
        public VolunteerEntityToVolunteerModel()
        {
            // todo vol id is not set correctly
            CreateMap<csc_Volunteer, Volunteer>()
                .ForMember(dest => dest.CanApplyCac, opt => opt.MapFrom(s => s.csc_IsCACVolunteer))
                .ForMember(dest => dest.CanApplyReac, opt => opt.MapFrom(s => s.csc_IsREACVolunteer))
                .ForMember(dest => dest.CanApplyCsc, opt => opt.MapFrom(s => s.csc_IsCSCVolunteer))
                .ForMember(dest => dest.EmailVerifiedOn, opt => opt.MapFrom(s => s.csc_EmailVerifiedOn))
                ;
        }
    }
}