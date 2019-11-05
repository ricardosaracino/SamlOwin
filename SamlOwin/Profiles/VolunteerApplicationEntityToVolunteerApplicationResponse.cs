using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerApplicationEntityToVolunteerApplicationResponse : Profile
    {
        public VolunteerApplicationEntityToVolunteerApplicationResponse()
        {
            CreateMap<csc_VolunteerApplication, VolunteerApplicationResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerApplicationId))
                .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(s => s.csc_ReferenceNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(s => s.StatusCodeEnum))
                .ForMember(dest => dest.ActivityApplyingFor, opt => opt.MapFrom(s => s.csc_ActivityApplyingForEnum))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(s => s.CreatedOn))
                ;
        }
    }
}