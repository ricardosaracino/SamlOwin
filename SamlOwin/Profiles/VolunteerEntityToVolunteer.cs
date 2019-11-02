using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEntityToVolunteer : Profile
    {
        public VolunteerEntityToVolunteer()
        {
            CreateMap<csc_Volunteer, Volunteer>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerId))
                .ForMember(dest => dest.CanApplyCac, opt => opt.MapFrom(s => s.csc_CanApplyCAC))
                .ForMember(dest => dest.CanApplyCsc, opt => opt.MapFrom(s => s.csc_CanApplyGeneral))
                .ForMember(dest => dest.CanApplyReac, opt => opt.MapFrom(s => s.csc_CanApplyREAC))
                .ForMember(dest => dest.EmailVerifiedOn, opt => opt.MapFrom(s => s.csc_EmailVerifiedOn))
                ;
        }
    }
}