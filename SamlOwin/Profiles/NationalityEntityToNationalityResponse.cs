using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class NationalityEntityToNationalityResponse : Profile
    {
        public NationalityEntityToNationalityResponse()
        {
            CreateMap<csc_Nationality, NationalityResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_NationalityId))
                .ForMember(dest => dest.EnLabel, opt => opt.MapFrom(s => s.csc_NationalityEnglish))
                .ForMember(dest => dest.FrLabel, opt => opt.MapFrom(s => s.csc_NationalityFrench))
                ;
        }
    }
}