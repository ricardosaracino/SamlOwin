using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity.Owin;
using SamlOwin.Handlers;
using SamlOwin.Identity;
using SamlOwin.Models;
using SamlOwin.Providers;

namespace SamlOwin.Controllers
{
    [RoutePrefix("api/volunteers")]
    public class VolunteerController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }
        
        /// <summary>
        /// Finds the Volunteer assigned to Current User
        /// </summary>
        /// <returns></returns>
        [VolunteerAuthorization]
        [HttpGet, Route("")]
        public WebApiSuccessResponse<Volunteer> Find()
        {
            var queryable = from cscVolunteer in _ctx.csc_VolunteerSet
                where cscVolunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteer;
            
            return new WebApiSuccessResponse<Volunteer>
            {
                Data =  _mapper.Map<Volunteer>(queryable.First())
            };
        }
    }
}