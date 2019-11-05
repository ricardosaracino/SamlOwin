using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class LocationEntityToLocationResponse: Profile
    {
        public LocationEntityToLocationResponse()
        {
            CreateMap<csc_Location, LocationResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_LocationId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_LocationEnglish))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_LocationFrench))
                ;
        }
    }
}