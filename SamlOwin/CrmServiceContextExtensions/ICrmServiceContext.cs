using CrmEarlyBound;

namespace SamlOwin.CrmServiceContextExtensions
{
    public interface ICrmServiceContext
    {
        CrmServiceContext GetCrmServiceContext();
    }
}