using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SamlOwin.GuidIdentity
{
    public abstract class ApplicationUser : IUser<Guid>
    {
        public abstract Guid Id { get; set; }

        public abstract string UserName { get; set; }

        public List<string> Roles { get; set; }

        public abstract Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager);

        public void AddRole(string role)
        {
            Roles.Add(role);
        }

        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    }
}