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
    }
}