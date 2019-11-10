using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Windows.Forms;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Xrm.Sdk;
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
    [RoutePrefix("api/volunteer-applications")]
    public class VolunteerApplicationController : ApiController, ICrmServiceContext
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerApplicationController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        public CrmServiceContext GetCrmServiceContext()
        {
            return _ctx;
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

        /// <summary>
        /// Finds the Volunteer Application assigned to Current User
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{volunteerApplicationId:Guid}")]
        [ResponseType(typeof(WebApiSuccessResponse<VolunteerApplicationResponse>))]
        public IHttpActionResult FindOne(Guid volunteerApplicationId)
        {
            var volunteerApplicationEntity =
                this.GetVolunteerApplicationEntity(volunteerApplicationId, User.Identity.GetVolunteerId());

            if (volunteerApplicationEntity == null)
            {
                return NotFound();
            }

            return Ok(new WebApiSuccessResponse<VolunteerApplicationResponse>
            {
                Data = _mapper.Map<VolunteerApplicationResponse>(volunteerApplicationEntity)
            });
        }

        /// <summary>
        /// Creates the Volunteer Application and assigns it to the Current User
        /// </summary>
        [HttpPost, Route("")]
        [ResponseType(typeof(WebApiSuccessResponse<VolunteerApplicationResponse>))]
        public IHttpActionResult Create(
            VolunteerApplicationRequest volunteerApplicationRequest)
        {
            csc_VolunteerApplication volunteerApplicationEntity;

            if (volunteerApplicationRequest.Id != null)
            {
                // attached to context
                volunteerApplicationEntity = this.GetVolunteerApplicationEntity(volunteerApplicationRequest.Id ?? Guid.Empty,
                    User.Identity.GetVolunteerId());

                if (volunteerApplicationEntity.StatusCodeEnum != csc_VolunteerApplication_StatusCode.Draft &&
                    volunteerApplicationEntity.StatusCodeEnum != csc_VolunteerApplication_StatusCode.Submitted)
                {
                    return Content(HttpStatusCode.BadRequest, new WebApiErrorResponse
                    {
                        Message = "The request is invalid.",
                        Data = "Volunteer Application StatusCode should be Daft or InProgress"
                    });
                }

                _mapper.Map(volunteerApplicationRequest, volunteerApplicationEntity);

                _ctx.UpdateObject(volunteerApplicationEntity);

                _ctx.SaveChanges();
            }
            else
            {
                volunteerApplicationEntity = new csc_VolunteerApplication();

                _ctx.AddObject(volunteerApplicationEntity);

                volunteerApplicationEntity.csc_Volunteer =
                    new EntityReference(csc_Volunteer.EntityLogicalName, User.Identity.GetVolunteerId());

                _mapper.Map(volunteerApplicationRequest, volunteerApplicationEntity);

                _ctx.SaveChanges();
            }

            // todo just return the id
            return Ok(new WebApiSuccessResponse<VolunteerApplicationResponse>
            {
                Data = _mapper.Map<VolunteerApplicationResponse>(volunteerApplicationEntity)
            });
        }
    }
}