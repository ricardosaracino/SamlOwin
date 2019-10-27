using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using SamlOwin.Identity;
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
                where cscPortalUser.csc_ProviderKey.Contains(login.ProviderKey)
                select new {cscPortalUser, cscVolunteer};

            var result = queryable.FirstOrDefault();

            if (result == null) return Task.FromResult(null as TUser);
            
            var portalUser = _mapper.Map<PortalUser>(result.cscPortalUser);
            portalUser.Volunteer = _mapper.Map<Volunteer>(result.cscVolunteer);

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
            
            var portalUser = _mapper.Map<PortalUser>(result.cscPortalUser);
            portalUser.Volunteer = _mapper.Map<Volunteer>(result.cscVolunteer);

            return Task.FromResult(portalUser as TUser);
        }
    }
}