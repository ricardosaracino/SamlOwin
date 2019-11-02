using System;
using System.Configuration;
using System.Data.Common;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace SamlOwin.Providers
{
    public static class CrmServiceContextProvider
    {
        private static IServiceManagement<IOrganizationService> _serviceManagement;
        private static AuthenticationCredentials _tokenCredentials;

        // http://www.cub-e.net/en/post/improve-microsoft-dynamics-365-crm-service-channel-allocation-performance
        public static CrmServiceContext Create()
        {
            if (_tokenCredentials == null)
            {
                var dbConnectionStringBuilder = new DbConnectionStringBuilder
                {
                    ConnectionString = ConfigurationManager.AppSettings["CrmConnectionString"]
                };

                var serviceUrl = $"{dbConnectionStringBuilder["Url"]}/XRMServices/2011/Organization.svc";

                _serviceManagement = (IServiceManagement<IOrganizationService>) ServiceConfigurationFactory
                    .CreateConfiguration<IOrganizationService>(new Uri(serviceUrl));

                _tokenCredentials = _serviceManagement.Authenticate(new AuthenticationCredentials()
                {
                    ClientCredentials =
                    {
                        UserName =
                        {
                            UserName = (string) dbConnectionStringBuilder["Username"],
                            Password = (string) dbConnectionStringBuilder["Password"],
                        }
                    }
                });
            }

            OrganizationServiceProxy service = null;

            switch (_serviceManagement.AuthenticationType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    service = new OrganizationServiceProxy(_serviceManagement, _tokenCredentials.ClientCredentials);
                    break;
                case AuthenticationProviderType.None:
                    break;
                case AuthenticationProviderType.Federation:
                    break;
                case AuthenticationProviderType.LiveId:
                    break;
                // using this for 365
                case AuthenticationProviderType.OnlineFederation:
                    service = new OrganizationServiceProxy(_serviceManagement, _tokenCredentials.SecurityTokenResponse);
                    break;
                default:
                    service = new OrganizationServiceProxy(_serviceManagement, _tokenCredentials.SecurityTokenResponse);
                    break;
            }

            if (service == null)
            {
                throw new NotImplementedException("AuthenticationProviderType");
            }

            service.EnableProxyTypes();

            return new CrmServiceContext(service);
        }
    }
}