using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using SamlOwin.GuidIdentity;
using SamlOwin.Providers;
using Serilog;

namespace SamlOwin.Models
{
    public sealed class PortalUserStore<TUser> : ApplicationUserStore<TUser> where TUser : ApplicationUser
    {
        private readonly CrmServiceContext _ctx;
        private readonly IMapper _mapper;

        public PortalUserStore(CrmServiceContext crmServiceContext)
        {
            _ctx = crmServiceContext;
            _mapper = AutoMapperProvider.GetMapper();
        }

        public override Task<TUser> FindAsync(UserLoginInfo login)
        {
            Log.Logger.Information("PortalUserStore.FindAsync");

            var queryable = from cscPortalUser in _ctx.csc_PortalUserSet
                join cscVolunteer in _ctx.csc_VolunteerSet on cscPortalUser.csc_Volunteer.Id equals cscVolunteer.Id
                    into gj
                from cscVolunteer in gj.DefaultIfEmpty()
                where cscPortalUser.csc_ProviderKey.Contains(login.ProviderKey) 
                      && cscPortalUser.csc_LoginProvider.Contains(login.LoginProvider)
                select new {cscPortalUser, cscVolunteer};

            var result = queryable.FirstOrDefault();

            csc_PortalUser portalUserEntity;
            csc_Volunteer volunteerEntity;

            if (result != null)
            {
                portalUserEntity = result.cscPortalUser;
                volunteerEntity = result.cscVolunteer;
                _ctx.Attach(portalUserEntity);
                portalUserEntity.csc_LastLoginDate = DateTime.Now;
                _ctx.UpdateObject(portalUserEntity);
                _ctx.SaveChanges();
            }
            else
            {
                portalUserEntity = new csc_PortalUser();
                volunteerEntity = new csc_Volunteer();
                _ctx.AddObject(portalUserEntity);
                portalUserEntity.csc_LoginProvider = login.LoginProvider;
                portalUserEntity.csc_ProviderKey = login.ProviderKey;
                portalUserEntity.csc_LastLoginDate = DateTime.Now;
                _ctx.SaveChanges();
            }

            var portalUser = _mapper.Map<PortalUser>(portalUserEntity);
            portalUser.Volunteer = _mapper.Map<Volunteer>(volunteerEntity);

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
                join cscVolunteer in _ctx.csc_VolunteerSet on cscPortalUser.csc_Volunteer.Id equals cscVolunteer.Id into
                    gj
                from cscVolunteer in gj.DefaultIfEmpty()
                where cscPortalUser.csc_PortalUserId.Equals(userId)
                select new {cscPortalUser, cscVolunteer};

            var result = queryable.FirstOrDefault();

            if (result == null) return Task.FromResult(null as TUser);

            var portalUser = _mapper.Map<PortalUser>(result.cscPortalUser);
            portalUser.Volunteer = _mapper.Map<Volunteer>(result.cscVolunteer);

            return Task.FromResult(portalUser as TUser);
        }
    }
}