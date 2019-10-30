using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SamlOwin.GuidIdentity;
using SamlOwin.Identity;

namespace SamlOwin.Models
{
    public sealed class PortalUser : ApplicationUser
    {
        public override Guid Id { get; set; }

        public override string UserName { get; set; }

        public string ProviderKey { get; set; }

        public string LoginProvider { get; set; }

        public Volunteer Volunteer { get; set; }

        public override async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            if (Volunteer.Id != Guid.Empty)
            {
                userIdentity.AddClaim(new Claim(CustomClaimTypes.VolunteerId, Volunteer.Id.ToString(),
                    ClaimValueTypes.String));
            }

            if (Volunteer.CanApplyCac == true)
            {
                userIdentity.AddClaim(new Claim(CustomClaimTypes.VolunteerCanApplyCac, "1", ClaimValueTypes.Integer));
            }

            if (Volunteer.CanApplyCsc == true)
            {
                userIdentity.AddClaim(new Claim(CustomClaimTypes.VolunteerCanApplyCsc, "1", ClaimValueTypes.Integer));
            }

            if (Volunteer.CanApplyReac == true)
            {
                userIdentity.AddClaim(new Claim(CustomClaimTypes.VolunteerCanApplyReac, "1", ClaimValueTypes.Integer));
            }

            if (Volunteer.EmailVerifiedOn != null)
            {
                userIdentity.AddClaim(new Claim(CustomClaimTypes.VolunteerEmailVerified, "1", ClaimValueTypes.Integer));
            }

            return userIdentity;
        }
    }
}