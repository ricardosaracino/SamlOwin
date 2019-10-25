using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Xrm.Sdk.Query;
using SamlOwin.Identity;
using XrmFramework.Attributes;

namespace SamlOwin.Models
{
    [Entity("csc_portaluser")]
    public sealed class PortalUser : ApplicationUser
    {
        [Id] 
        [Column("csc_portaluserid")] 
        public override Guid Id { get; set; }

        [Name] 
        [Column("csc_name")] 
        public override string UserName { get; set; }

        [Column("csc_providerkey")] 
        public string ProviderKey { get; set; }

        [Column("csc_loginprovider")] 
        public string LoginProvider { get; set; }

        [Link(JoinOperator = JoinOperator.LeftOuter)]
        [Column("csc_volunteer", AttributeType = "LookupType")]
        public Volunteer Volunteer { get; set; }
        
        public override async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            if (Volunteer?.Id != null)
            {
                userIdentity.AddClaim(new Claim("volunteer.id", Volunteer.Id.ToString()));
            }

            if (Volunteer?.CanApplyCac == true)
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
            }
            
            return userIdentity;
        }
    }
}