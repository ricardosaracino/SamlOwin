using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using Microsoft.Xrm.Sdk.Query;
using SamlOwin.Identity;
using XrmFramework.Attributes;

namespace SamlOwin.Models
{
    [Entity("csc_portaluser")]
    public class ApplicationUser : csc_PortalUser, IUser<Guid>
    {

        public string UserName { get; set; }
        
        public List<string> Roles { get; set; }
        
        public void AddRole(string role)
        {
            Roles.Add(role);
        }

        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    
        
        public  async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            /*
            if (csc_Volunteer?.Id != null)
            {
                userIdentity.AddClaim(new Claim("volunteer.id", csc_Volunteer.Id.ToString()));
            }

            if (csc_Volunteer.CanApplyCac == true)
            {
                userIdentity.AddClaim(new Claim("volunteer.canApplyCac", "1"));
            }

            if (Volunteer?.CanApplyCsc == true)
            {
                userIdentity.AddClaim(new Claim("volunteer.canApplyCsc", "1"));
            }

            if (Volunteer?.CanApplyReac == true)
            {
                userIdentity.AddClaim(new Claim("volunteer.canApplyReac", "1"));
            }

            if (Volunteer?.EmailVerifiedOn != null)
            {
                userIdentity.AddClaim(new Claim("volunteer.emailVerified", "1"));
            }*/
            
            return userIdentity;
        }
    }
}