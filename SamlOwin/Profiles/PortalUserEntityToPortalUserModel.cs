using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
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
}