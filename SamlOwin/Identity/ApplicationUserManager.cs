using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

/**
 * https://stackoverflow.com/questions/22652543/does-new-asp-net-mvc-identity-framework-work-without-entity-framework-and-sql-se
 */
namespace SamlOwin.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store)
            : base(store)
        {
        }

        public async Task<IdentityResult> AddUserToRolesAsync(Guid userId, IEnumerable<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, Guid>) Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);

            if (user == null) throw new InvalidOperationException("Invalid user Id");

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            // Add user to each role using UserRoleStore
            foreach (var role in roles.Where(role => !userRoles.Contains(role)))
                await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);

            // Call update once when all roles are added
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<IdentityResult> RemoveUserFromRolesAsync(Guid userId, IEnumerable<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, Guid>) Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new InvalidOperationException("Invalid user Id");

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            // Remove user to each role using UserRoleStore
            foreach (var role in roles.Where(userRoles.Contains))
                await userRoleStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);

            // Call update once when all roles are removed
            return await UpdateAsync(user).ConfigureAwait(false);
        }
    }
}