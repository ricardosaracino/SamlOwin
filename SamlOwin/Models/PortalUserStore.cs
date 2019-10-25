using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using SamlOwin.Identity;
using Serilog;
using Serilog.Core;

namespace SamlOwin.Models
{
    public sealed class PortalUserStore<TUser> : ApplicationUserStore<TUser> where TUser : ApplicationUser
    {
        private readonly CrmServiceClient client;

        // https://github.com/poloagustin/poloagustin-bsv/blob/master/ACCENDO/Accendo.DynamicsIntegration.Crm/Accendo.DynamicsIntegration.Crm2015/CrmConfiguration.cs
        // https://github.com/poloagustin/poloagustin-bsv/blob/master/ACCENDO/Accendo.DynamicsIntegration.Crm/Accendo.DynamicsIntegration.Crm2015/app.config
        public OrganizationServiceProxy GetProxy()
        {
            var credentials = new ClientCredentials()
            {
                UserName =
                {
                    UserName = "CRM365.service@053gc.onmicrosoft.com", Password = ""
                }
            };

            return new OrganizationServiceProxy(
                new Uri("https://dev-csc-scc.crm3.dynamics.com/XRMServices/2011/Organization.svc"), null, credentials,
                null);
        }


        public PortalUserStore(CrmServiceClient client)
        {
            this.client = client;
        }

        public override Task<TUser> FindAsync(UserLoginInfo login)
        {
            var query = new QueryExpression
            {
                EntityName = csc_PortalUser.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression()
                        {
                            AttributeName = "csc_providerkey",
                            Operator = ConditionOperator.Equal,
                            Values = {login.ProviderKey}
                        }
                    }
                },
                LinkEntities =
                {
                    new LinkEntity(csc_PortalUser.EntityLogicalName, csc_Volunteer.EntityLogicalName, "csc_volunteer",
                        "csc_volunteer", JoinOperator.LeftOuter)
                }
            };

            try
            {
                using (var serviceProxy = GetProxy())
                {
                    serviceProxy.EnableProxyTypes();

                    var service = (IOrganizationService) serviceProxy;

                    var entity = service.RetrieveMultiple(query)
                        .Entities
                        .Select(e => e.ToEntity<ApplicationUser>())
                        .First();


                    //entity.csc_LastLoginDate = DateTime.Now;

                    //serviceProxy.Update(entity);


                    /*var entity = client.RetrieveMultiple(query)
                        .Entities
                        .Select(e => e.ToEntity<ApplicationUser>())
                        .First();
                    
                    return Task.FromResult(entity as TUser);*/


                    /*var portalUser = new csc_PortalUser()
                    {
                        csc_LoginProvider = login.LoginProvider
                    };

                    service.Create(portalUser);*/


                    var context =
                        new OrganizationServiceContext(service);

                    context.LoadProperty(entity, "csc_Volunteer");

                  //  csc_PortalUser portalUser = context..Where(c => c.FirstName == "Pamela").FirstOrDefault();

                    return Task.FromResult(entity as TUser);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }

            return Task.FromResult(null as TUser);
        }

        public override Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            // TODO why the cast
            return Task.FromResult(new List<Claim>() as IList<Claim>);
        }

        public override Task<TUser> FindByIdAsync(Guid userId)
        {
            try
            {
                using (var serviceProxy = GetProxy())
                {
                    serviceProxy.EnableProxyTypes();

                    var service = (IOrganizationService) serviceProxy;

                    var entity = service.Retrieve(csc_PortalUser.EntityLogicalName, userId,
                            new ColumnSet(true))
                        .ToEntity<ApplicationUser>();

                    //entity.csc_LastLoginDate = DateTime.Now;

                    //serviceProxy.Update(entity);

                    return Task.FromResult(entity as TUser);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }

            return Task.FromResult(null as TUser);
        }
    }
}