using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Xrm.Sdk.Query;
using XrmFramework;

namespace SamlOwin.Models
{
    public class UserStore<TUser> : IUserStore<TUser>,
        IUserRoleStore<TUser>,
        IUserLoginStore<TUser>
        where TUser : ApplicationUser
    {
        private XrmContext _context;

        public UserStore(XrmContext context)
        {
            _context = context;
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            var queryExpression = _context.BuildQuery<ApplicationUser>();
            
            var user = _context.FindFirst<ApplicationUser>(queryExpression);
            
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            user.AddRole(roleName);
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            user.RemoveRole(roleName);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.FromResult((IList<string>) user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public void Dispose()
        {
        }

        public Task CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}