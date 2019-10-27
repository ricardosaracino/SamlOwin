using AutoMapper;
using AutoMapper.Configuration;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin
{
    public static class AutoMapperProvider
    {
        private static MapperConfiguration _mapperConfiguration;

        public static Mapper GetMapper()
        {
            if (_mapperConfiguration == null)
            {
                _mapperConfiguration = CreateConfiguration();
            }

            return new Mapper(_mapperConfiguration);
        }

        private static MapperConfiguration CreateConfiguration()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<PortalUserEntityToPortalUserModel>();
            cfg.AddProfile<VolunteerEntityToVolunteerModel>();
            return new MapperConfiguration(cfg);
        }
    }


    public class PortalUserEntityToPortalUserModel : Profile
    {
        public PortalUserEntityToPortalUserModel()
        {
            CreateMap<csc_PortalUser, PortalUser>()
                .ForMember(dest => dest.LoginProvider, opt => opt.MapFrom(s => s.csc_LoginProvider))
                .ForMember(dest => dest.ProviderKey, opt => opt.MapFrom(s => s.csc_ProviderKey))
                ;
        }
    }

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