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
    [RoutePrefix("api/volunteer-languages")]
    public class VolunteerLanguageController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerLanguageController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Volunteer Languages assigned to Current User
        /// </summary>
        [VolunteerAuthorization]
        [HttpGet, Route("")]
        public  WebApiSuccessResponse<List<VolunteerLanguageResponse>> FindAll()
        {
            var queryable = from volunteerLanguageEntity in _ctx.csc_VolunteerLanguageSet
                orderby volunteerLanguageEntity.CreatedOn descending
                where volunteerLanguageEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select volunteerLanguageEntity;
            
            return new WebApiSuccessResponse<List<VolunteerLanguageResponse>>
            {
                Data = queryable.ToList().ConvertAll(entity => _mapper.Map<VolunteerLanguageResponse>(entity))
            };
        }

        /// <summary>
        /// Updates or Creates all Volunteer Languages and assigns them to the current user
        /// </summary>
        [VolunteerAuthorization]
        [HttpPost, Route("")]
        public WebApiSuccessResponse Save(List<VolunteerLanguageRequest> volunteerLanguages)
        {
            foreach (var volunteerLanguage in volunteerLanguages)
            {
                if (volunteerLanguage.Id != null && volunteerLanguage.Id != Guid.Empty) continue;

                var volunteerEntity = _mapper.Map<csc_VolunteerLanguage>(volunteerLanguage);

                volunteerEntity.csc_Volunteer = new EntityReference(csc_Volunteer.EntityLogicalName,
                    User.Identity.GetVolunteerId());

                _ctx.AddObject(volunteerEntity);
            }

            var queryable = from volunteerLanguageEntity in _ctx.csc_VolunteerLanguageSet
                where volunteerLanguageEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select volunteerLanguageEntity;

            var volunteerLanguageEntities = queryable.ToList();

            foreach (var volunteerLanguageEntity in volunteerLanguageEntities)
            {
                var foundVolunteerLanguage =
                    volunteerLanguages.SingleOrDefault(req => req.Id == volunteerLanguageEntity.Id);

                if (foundVolunteerLanguage != null)
                {
                    _ctx.UpdateObject(_mapper.Map(foundVolunteerLanguage, volunteerLanguageEntity));
                }
                else
                {
                    _ctx.DeleteObject(volunteerLanguageEntity);
                }
            }

            _ctx.SaveChanges();

            return new WebApiSuccessResponse();
        }
    }
}