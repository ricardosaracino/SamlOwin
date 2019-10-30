using System;
using System.Security.Claims;
using System.Security.Principal;
using SamlOwin.Models;

namespace SamlOwin.Identity
{
    public static class IdentityExtensions
    {
        public static Guid GetVolunteerId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var claim = claimsIdentity?.FindFirst(CustomClaimTypes.VolunteerId);

            return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
        }
    }
}