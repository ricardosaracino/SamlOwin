using System;
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
    /// <summary>
    /// Volunteer Authorize Permission
    /// </summary>
    [VolunteerAuthorize]
    [RoutePrefix("api/volunteer-self-identifications")]
    public class VolunteerSelfIdentificationController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerSelfIdentificationController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        private csc_Volunteer GetVolunteerEntityById(Guid id)
        {
            var queryable = from volunteerEntity in _ctx.csc_VolunteerSet
                where volunteerEntity.Id.Equals(id)
                select volunteerEntity;

            return queryable.Single();
        }

        /// <summary>
        /// Finds the Volunteer Self Identification to Current User
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<VolunteerSelfIdentificationResponse> Find()
        {
            return new WebApiSuccessResponse<VolunteerSelfIdentificationResponse>
            {
                Data = _mapper.Map<VolunteerSelfIdentificationResponse>(GetVolunteerEntityById(User.Identity.GetVolunteerId()))
            };
        }

        /// <summary>
        /// Updates or Creates the Volunteer Self Identification assigned to the Current User
        /// </summary>
        [HttpPost, Route("")]
        public WebApiSuccessResponse Save(VolunteerSelfIdentificationRequest volunteerSelfIdentificationRequest)
        {
            var volunteerEntity = GetVolunteerEntityById(User.Identity.GetVolunteerId());

            _ctx.UpdateObject(_mapper.Map(volunteerSelfIdentificationRequest, volunteerEntity));

            _ctx.SaveChanges();

            return new WebApiSuccessResponse();
        }
    }
}