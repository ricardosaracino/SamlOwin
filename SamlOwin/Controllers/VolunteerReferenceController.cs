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
    [RoutePrefix("api/volunteer-references")]
    public class VolunteerReferenceController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerReferenceController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Volunteer References assigned to Current User
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<VolunteerReferenceResponse>> FindAll()
        {
            var queryable = from cscVolunteerReferenceEntity in _ctx.csc_VolunteerReferenceSet
                orderby cscVolunteerReferenceEntity.CreatedOn descending
                where cscVolunteerReferenceEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteerReferenceEntity;

            return new WebApiSuccessResponse<List<VolunteerReferenceResponse>>
            {
                Data = queryable.ToList().ConvertAll(entity => _mapper.Map<VolunteerReferenceResponse>(entity))
            };
        }
        
        /// <summary>
        /// Updates or Creates all Volunteer References and assigns them to the Current User
        /// </summary>
        [HttpPost, Route("")]
        public WebApiSuccessResponse Save(List<VolunteerReferenceRequest> volunteerReferences)
        {
            foreach (var volunteerReference in volunteerReferences)
            {
                if (volunteerReference.Id != null && volunteerReference.Id != Guid.Empty) continue;

                var volunteerEntity = _mapper.Map<csc_VolunteerReference>(volunteerReference);

                volunteerEntity.csc_Volunteer =
                    new EntityReference(csc_Volunteer.EntityLogicalName, User.Identity.GetVolunteerId());

                _ctx.AddObject(volunteerEntity);
            }

            var queryable = from volunteerReferenceEntity in _ctx.csc_VolunteerReferenceSet
                where volunteerReferenceEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select volunteerReferenceEntity;

            var volunteerReferenceEntities = queryable.ToList();

            foreach (var volunteerReferenceEntity in volunteerReferenceEntities)
            {
                var foundVolunteerReference =
                    volunteerReferences.SingleOrDefault(req => req.Id == volunteerReferenceEntity.Id);

                if (foundVolunteerReference != null)
                {
                    _ctx.UpdateObject(_mapper.Map(foundVolunteerReference, volunteerReferenceEntity));
                }
                else
                {
                    _ctx.DeleteObject(volunteerReferenceEntity);
                }
            }

            _ctx.SaveChanges();

            return new WebApiSuccessResponse();
        }
    }
}