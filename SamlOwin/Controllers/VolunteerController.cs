using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity.Owin;
using SamlOwin.CrmServiceContextExtensions;
using SamlOwin.Handlers;
using SamlOwin.Identity;
using SamlOwin.Models;
using SamlOwin.Providers;

namespace SamlOwin.Controllers
{
    /// <summary>
    /// Volunteer Authorize Permission
    /// </summary>
    [VolunteerAuthorize]
    [RoutePrefix("api/volunteers")]
    public class VolunteerController : ApiController, ICrmServiceContext
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        public CrmServiceContext GetCrmServiceContext()
        {
            return _ctx;
        }

        /// <summary>
        /// Finds the Volunteer assigned to Current User
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<VolunteerResponse> Find()
        {
            return new WebApiSuccessResponse<VolunteerResponse>
            {
                Data = _mapper.Map<VolunteerResponse>(this.GetVolunteerEntity(User.Identity.GetVolunteerId()))
            };
        }

        /// <summary>
        /// Updates or Creates the Volunteer assigned to the Current User
        /// </summary>
        [HttpPost, Route("")]
        public WebApiSuccessResponse Save(VolunteerRequest volunteerRequest)
        {
            var volunteerEntity = this.GetVolunteerEntity(User.Identity.GetVolunteerId());

            _ctx.UpdateObject(_mapper.Map(volunteerRequest, volunteerEntity));

            _ctx.SaveChanges();

            return new WebApiSuccessResponse();
        }

    }
}