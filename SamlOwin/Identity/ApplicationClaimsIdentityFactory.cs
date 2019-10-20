﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SamlOwin.Models;

namespace SamlOwin.Identity
{
    public class ApplicationClaimsIdentityFactory : ClaimsIdentityFactory<ApplicationUser, Guid>
    {
        public override Task<ClaimsIdentity> CreateAsync(UserManager<ApplicationUser, Guid> manager,
            ApplicationUser user, string authenticationType)
        {
            var id = new ClaimsIdentity(authenticationType, UserNameClaimType, RoleClaimType);

            // An item with the same key has already been added.

            id.AddClaim(new Claim(UserIdClaimType, ConvertIdToString(user.Id),
                "http://www.w3.org/2001/XMLSchema#string"));

            id.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));

            // returns too
            return Task.FromResult(id);
        }
    }
}