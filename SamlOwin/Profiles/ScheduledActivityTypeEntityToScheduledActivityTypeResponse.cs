using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class ScheduledActivityTypeEntityToScheduledActivityTypeResponse : Profile
    {
        public ScheduledActivityTypeEntityToScheduledActivityTypeResponse()
        {
            CreateMap<csc_ActivityType, ScheduledActivityTypeResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_ActivityTypeId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_NameEN))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_NameFR))
                ;
        }
    }
}