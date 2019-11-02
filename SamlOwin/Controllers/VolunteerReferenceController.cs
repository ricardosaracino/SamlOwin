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
        /// Finds all <c>VolunteerReferenceResponse</c> assigned to Current User
        /// </summary>
        /// <seealso cref="VolunteerReferenceResponse"/>
        [VolunteerAuthorization]
        [HttpGet, Route("")]
        public WebApiSuccessResponse<IEnumerable<VolunteerReferenceResponse>> FindAll()
        {
            var queryable = from cscVolunteerReferenceEntity in _ctx.csc_VolunteerReferenceSet
                orderby cscVolunteerReferenceEntity.CreatedOn descending
                where cscVolunteerReferenceEntity.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteerReferenceEntity;

            return new WebApiSuccessResponse<IEnumerable<VolunteerReferenceResponse>>
            {
                Data = queryable.ToList().ConvertAll(entity => _mapper.Map<VolunteerReferenceResponse>(entity))
            };
        }
    }
}