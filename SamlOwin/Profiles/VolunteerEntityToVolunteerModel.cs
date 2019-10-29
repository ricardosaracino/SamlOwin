using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEntityToVolunteerModel : Profile
    {
        public VolunteerEntityToVolunteerModel()
        {
            CreateMap<csc_Volunteer, Volunteer>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerId))
                .ForMember(dest => dest.CanApplyCac, opt => opt.MapFrom(s => s.csc_CanApplyCAC))
                .ForMember(dest => dest.CanApplyCsc, opt => opt.MapFrom(s => s.csc_CanApplyGeneral))
                .ForMember(dest => dest.CanApplyReac, opt => opt.MapFrom(s => s.csc_CanApplyREAC))
                .ForMember(dest => dest.EmailVerifiedOn, opt => opt.MapFrom(s => s.csc_EmailVerifiedOn))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.csc_FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(s => s.csc_LastName))
                .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(s => s.csc_ReferenceNumber))
                ;
        }
    }
}