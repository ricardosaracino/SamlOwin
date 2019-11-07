using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/volunteer-applications")]
    public class VolunteerApplicationController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerApplicationController()
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
        /// Finds the Volunteer Applications assigned to Current User
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<VolunteerApplicationResponse>> FindAll()
        {
            var queryable = from volunteerApplicationEntity in _ctx.csc_VolunteerApplicationSet
                where volunteerApplicationEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select volunteerApplicationEntity;

            return new WebApiSuccessResponse<List<VolunteerApplicationResponse>>
            {
                Data = queryable.ToList().ConvertAll(entity => _mapper.Map<VolunteerApplicationResponse>(entity))
            };
        }
    }
}