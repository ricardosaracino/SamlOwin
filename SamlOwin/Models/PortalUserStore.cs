using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using SamlOwin.Identity;
using Serilog;

namespace SamlOwin.Models
{
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
                select new PortalUser()
                {
                    Id = cscPortalUser.Id,
                    LoginProvider = cscPortalUser.csc_LoginProvider,
                    ProviderKey = cscPortalUser.csc_ProviderKey,
                    Volunteer = new Volunteer()
                    {
                        Id = cscVolunteer.Id,
                        CanApplyCac = cscVolunteer.csc_IsCACVolunteer,
                        CanApplyReac = cscVolunteer.csc_IsREACVolunteer,
                        CanApplyCsc = cscVolunteer.csc_IsCSCVolunteer,
                        EmailVerifiedOn = cscVolunteer.csc_EmailVerifiedOn,
                    }
                };

            return Task.FromResult(queryable.FirstOrDefault() as TUser);
                /*select new {cscPortalUser = pu, cscVolunteer = v};

            var result = queryable.FirstOrDefault();

            if (result == null)
                return Task.FromResult(null as TUser);

            var cscPortalUser = result.cscPortalUser;

            _ctx.Attach(cscPortalUser);

            cscPortalUser.csc_LastLoginDate = DateTime.Now.AddDays(3);

            _ctx.UpdateObject(cscPortalUser);

            _ctx.SaveChanges();

            return Task.FromResult(Transformer.GetModel<PortalUser>(cscPortalUser) as TUser);*/
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
                select new PortalUser()
                {
                    Id = cscPortalUser.Id,
                    LoginProvider = cscPortalUser.csc_LoginProvider,
                    ProviderKey = cscPortalUser.csc_ProviderKey,
                    Volunteer = new Volunteer()
                    {
                        Id = cscVolunteer.Id,
                        CanApplyCac = cscVolunteer.csc_IsCACVolunteer,
                        CanApplyReac = cscVolunteer.csc_IsREACVolunteer,
                        CanApplyCsc = cscVolunteer.csc_IsCSCVolunteer,
                        EmailVerifiedOn = cscVolunteer.csc_EmailVerifiedOn,
                    }
                };

            return Task.FromResult(queryable.FirstOrDefault() as TUser);
        }
    }
}