using System.Web.Http.ExceptionHandling;

namespace SamlOwin.Handlers
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Serilog.Log.Logger.Error(context.Exception, "");
            
            base.Log(context);
        }
    }
}