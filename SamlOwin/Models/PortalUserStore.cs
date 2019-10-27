using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using SamlOwin.Identity;
using Serilog;

namespace SamlOwin.Models
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

    public sealed class PortalUserStore<TUser> : ApplicationUserStore<TUser> where TUser : ApplicationUser
    {
        private readonly CrmServiceContext _ctx;

        public PortalUserStore(CrmServiceContext crmServiceContext)
        {
            _ctx = crmServiceContext;
        }

        public override Task<TUser> FindAsync(UserLoginInfo login)
        {
            Log.Logger.Information("PortalUserStore.FindAsync");

            var queryable = from cscPortalUser in _ctx.csc_PortalUserSet
                join cscVolunteer in _ctx.csc_VolunteerSet on cscPortalUser.csc_Volunteer.Id equals cscVolunteer.Id
                where cscPortalUser.csc_ProviderKey.Contains(login.ProviderKey)
                select new {cscPortalUser, cscVolunteer};

            var result = queryable.FirstOrDefault();

            if (result == null) return Task.FromResult(null as TUser);

            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<PortalUserEntityToPortalUserModel>();
            cfg.AddProfile<VolunteerEntityToVolunteerModel>();
            var mapperConfig = new MapperConfiguration(cfg);
            IMapper mapper = new Mapper(mapperConfig);

            var portalUser = mapper.Map<PortalUser>(result.cscPortalUser);
            portalUser.Volunteer = mapper.Map<Volunteer>(result.cscVolunteer);

            _ctx.Attach(result.cscPortalUser);

            result.cscPortalUser.csc_LastLoginDate = DateTime.Now.AddDays(3);

            _ctx.UpdateObject(result.cscPortalUser);

            _ctx.SaveChanges();

            return Task.FromResult(portalUser as TUser);
        }

        public override Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            return Task.FromResult(new List<Claim>() as IList<Claim>);
        }

        public override Task<TUser> FindByIdAsync(Guid userId)
        {
            Log.Logger.Information("PortalUserStore.FindByIdAsync");

            var queryable = from cscPortalUser in _ctx.csc_PortalUserSet
                join cscVolunteer in _ctx.csc_VolunteerSet on cscPortalUser.csc_Volunteer.Id equals cscVolunteer.Id
                where cscPortalUser.csc_PortalUserId.Equals(userId)
                select new {cscPortalUser, cscVolunteer};

            var result = queryable.FirstOrDefault();

            if (result == null) return Task.FromResult(null as TUser);

            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<PortalUserEntityToPortalUserModel>();
            cfg.AddProfile<VolunteerEntityToVolunteerModel>();
            var mapperConfig = new MapperConfiguration(cfg);
            IMapper mapper = new Mapper(mapperConfig);

            var portalUser = mapper.Map<PortalUser>(result.cscPortalUser);
            portalUser.Volunteer = mapper.Map<Volunteer>(result.cscVolunteer);

            return Task.FromResult(portalUser as TUser);
        }
    }
}