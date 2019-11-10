using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Xrm.Sdk;
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
    [RoutePrefix("api/volunteer-emergency-contacts")]
    public class VolunteerEmergencyContactController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerEmergencyContactController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Volunteer Emergency Contacts assigned to Current User
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<VolunteerEmergencyContactResponse>> FindAll()
        {
            var queryable = from volunteerEmergencyContactEntity in _ctx.csc_VolunteerEmergencyContactSet
                orderby volunteerEmergencyContactEntity.CreatedOn descending
                where volunteerEmergencyContactEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select volunteerEmergencyContactEntity;

            return new WebApiSuccessResponse<List<VolunteerEmergencyContactResponse>>
            {
                Data = queryable.ToList().ConvertAll(entity => _mapper.Map<VolunteerEmergencyContactResponse>(entity))
            };
        }

        /// <summary>
        /// Updates or Creates all Volunteer Emergency Contacts and assigns them to the Current User
        /// </summary>
        [HttpPost, Route("")]
        public WebApiSuccessResponse Save(List<VolunteerEmergencyContactRequest> volunteerEmergencyContacts)
        {
            foreach (var volunteerEmergencyContact in volunteerEmergencyContacts)
            {
                if (volunteerEmergencyContact.Id != null && volunteerEmergencyContact.Id != Guid.Empty) continue;

                var volunteerEntity = _mapper.Map<csc_VolunteerEmergencyContact>(volunteerEmergencyContact);

                volunteerEntity.csc_Volunteer =
                    new EntityReference(csc_Volunteer.EntityLogicalName, User.Identity.GetVolunteerId());

                _ctx.AddObject(volunteerEntity);
            }

            var queryable = from volunteerEmergencyContactEntity in _ctx.csc_VolunteerEmergencyContactSet
                where volunteerEmergencyContactEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select volunteerEmergencyContactEntity;

            var volunteerEmergencyContactEntities = queryable.ToList();

            foreach (var volunteerEmergencyContactEntity in volunteerEmergencyContactEntities)
            {
                var foundVolunteerEmergencyContact =
                    volunteerEmergencyContacts.SingleOrDefault(req => req.Id == volunteerEmergencyContactEntity.Id);

                if (foundVolunteerEmergencyContact != null)
                {
                    _ctx.UpdateObject(_mapper.Map(foundVolunteerEmergencyContact, volunteerEmergencyContactEntity));
                }
                else
                {
                    _ctx.DeleteObject(volunteerEmergencyContactEntity);
                }
            }

            _ctx.SaveChanges();

            return new WebApiSuccessResponse();
        }
    }
}