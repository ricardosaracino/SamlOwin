using System.Configuration;
using CrmEarlyBound;
using Microsoft.Xrm.Tooling.Connector;

namespace SamlOwin
{
    public class CrmServiceContextProvider
    {
        private static CrmServiceClient _crmServiceClient;
        
        public static CrmServiceContext Create()
        {
            if (_crmServiceClient?.IsReady == null)
            {

                _crmServiceClient = new CrmServiceClient(ConfigurationManager.AppSettings["CrmConnectionString"]);

                _crmServiceClient.OrganizationServiceProxy.EnableProxyTypes();
            }

            return new CrmServiceContext(_crmServiceClient.OrganizationServiceProxy);
        }
    }
}