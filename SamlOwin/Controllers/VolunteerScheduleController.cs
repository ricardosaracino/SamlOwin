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
    [VolunteerAuthorization]
    [RoutePrefix("api/volunteer-schedules")]
    public class VolunteerScheduleController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerScheduleController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds the Volunteer Schedules assigned to Current User
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<VolunteerScheduleResponse>> FindAll()
        {
            var queryable = from volunteerScheduleEntity in _ctx.csc_ScheduleSet
                join scheduledActivityEntity in _ctx.csc_ScheduledActivitySet on volunteerScheduleEntity
                        .csc_ScheduledActivity.Id equals
                    scheduledActivityEntity.Id
                join locationEntity in _ctx.csc_LocationSet on scheduledActivityEntity.csc_Location.Id equals
                    locationEntity.Id
                join activityTypeEntity in _ctx.csc_ActivityTypeSet on scheduledActivityEntity.csc_ActivityType.Id
                    equals
                    activityTypeEntity.Id
                where volunteerScheduleEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select new {volunteerScheduleEntity, locationEntity, activityTypeEntity};

            return new WebApiSuccessResponse<List<VolunteerScheduleResponse>>
            {
                Data = queryable.ToList().ConvertAll(result => _mapper.Map(result.volunteerScheduleEntity,
                    new VolunteerScheduleResponse
                    {
                        Location = _mapper.Map<LocationResponse>(result.locationEntity),
                        ScheduledActivityType = _mapper.Map<ScheduledActivityTypeResponse>(result.activityTypeEntity),
                    }))
            };
        }
    }
}