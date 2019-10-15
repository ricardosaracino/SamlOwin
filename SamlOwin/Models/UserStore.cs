using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Xrm.Sdk.Query;
using XrmFramework;

namespace SamlOwin.Models
{
    public class UserStore<TUser> : AbstractStore<TUser> where TUser : ApplicationUser
    {
        private readonly XrmService _xrm;

        public UserStore(XrmService xrm)
        {
            _xrm = xrm;
        }

        public override Task<TUser> FindAsync(UserLoginInfo login)
        {
            // TODO refactor
            var queryExpression = _xrm.BuildQuery<ApplicationUser>();

            queryExpression.Criteria.AddCondition(new ConditionExpression("csc_providerkey", ConditionOperator.Equal,
                login.ProviderKey));

            var user = _xrm.FindFirst<ApplicationUser>(queryExpression);

            // TODO why the cast
            return Task.FromResult(user as TUser);
        }

        public override Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            // TODO why the cast
            return Task.FromResult(new List<Claim>() as IList<Claim>);
        }

        public override Task<TUser> FindByIdAsync(Guid userId)
        {
            // TODO refactor
            var queryExpression = _xrm.BuildQuery<ApplicationUser>();

            queryExpression.Criteria.AddCondition(new ConditionExpression("csc_portaluserid", ConditionOperator.Equal,
                userId));

            var user = _xrm.FindFirst<ApplicationUser>(queryExpression);

            // TODO why the cast
            return Task.FromResult(user as TUser);
        }
    }
}