using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Net;
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
        /// <seealso cref="VolunteerReferenceResponse"/>
        [VolunteerAuthorization]
        [HttpGet, Route("find-all")]
        public ApiResponse<VolunteerLanguagesControllerFindAllResponse> FindAll()
        {
            var queryable = from cscVolunteerLanguage in _ctx.csc_VolunteerLanguageSet
                orderby cscVolunteerLanguage.CreatedOn descending
                where cscVolunteerLanguage.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteerLanguage;

            return new ApiSuccessResponse<VolunteerLanguagesControllerFindAllResponse>
            {
                Data = new VolunteerLanguagesControllerFindAllResponse
                {
                    VolunteerLanguages = queryable.ToList()
                        .ConvertAll(entity => _mapper.Map<VolunteerLanguageResponse>(entity))
                }
            };
        }


        /// <summary>
        /// Updates or Creates all Volunteer Languages and assigns them to the current user
        /// </summary>
        /// <seealso cref="VolunteerLanguageRequest"/>
        [VolunteerAuthorization]
        [HttpPost, Route("save")]
        public ApiResponse<object> Save([FromBody] IEnumerable<VolunteerLanguageRequest> volunteerLanguageRequest)
        {
            return new ApiSuccessResponse<object>
            {
                Data = null
            };
        }
    }
}