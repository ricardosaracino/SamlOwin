using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using XrmFramework.Attributes;

namespace SamlOwin.Models
{
    [Entity("csc_portaluser")]
    public class ApplicationUser : IUser<Guid>
    {
        [Column("csc_providerkey")] public string ProviderKey { get; set; }

        [Column("csc_loginprovider")] public string LoginProvider { get; set; }

        public List<string> Roles { get; set; }

        [Id] [Column("csc_portaluserid")] public Guid Id { get; set; }

        [Name] [Column("csc_name")] public string UserName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    }
}