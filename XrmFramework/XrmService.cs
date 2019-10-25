using System;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace XrmFramework
{
    public class XrmService: IDisposable
    {
        private static CrmServiceClient _client;

        public void Dispose()
        {
        }
        public static XrmService Create(string crmConnectionString)
        {
            if (_client?.IsReady == null)
            {
                _client = new CrmServiceClient(crmConnectionString);
            }
            
            // todo what about if IsReady is false
            
            return new XrmService();
        }
        
        public QueryExpression BuildQuery<T>()
        {
            return QueryBuilder.Build<T>();
        }
        
        public T Find<T>(string id)
        {
            return default(T);
        }
        
        public T FindFirst<T>(QueryExpression queryExpression)
        {
            var entityCollection = _client.RetrieveMultiple(queryExpression);

            foreach (var entity in entityCollection.Entities)
            {
                return Transformer.GetModel<T>(entity);
            }

            return default(T);
        }
    }
}