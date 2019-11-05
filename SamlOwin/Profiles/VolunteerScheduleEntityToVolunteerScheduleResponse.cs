using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerScheduleEntityToVolunteerScheduleResponse: Profile
    {
        public VolunteerScheduleEntityToVolunteerScheduleResponse()
        {
            CreateMap<csc_Schedule, VolunteerScheduleResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_ScheduleId))
                .ForMember(dest => dest.ShiftStart, opt => opt.MapFrom(s => s.csc_ShiftStart))
                .ForMember(dest => dest.ShiftEnd, opt => opt.MapFrom(s => s.csc_ShiftEnd))
                .ForMember(dest => dest.Volunteer, opt => opt.MapFrom(s => s.csc_Volunteer))
                ;
        }
    }
}