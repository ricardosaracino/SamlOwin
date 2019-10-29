using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity.Owin;
using SamlOwin.Models;
using SamlOwin.Providers;

namespace SamlOwin.Controllers
{
    public class VolunteerController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }
        
        [AllowAnonymous]
        [HttpGet]
        [ActionName("FindFirst")]
        public object FindFirst()
        {
            var queryable = from v in _ctx.csc_VolunteerSet
                orderby v.CreatedOn descending
                select v;

            var cscVolunteer = queryable.Take(1).FirstOrDefault();
            
            return _mapper.Map<Volunteer>(cscVolunteer);
        }
    }
}