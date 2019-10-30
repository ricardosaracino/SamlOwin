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
    public class VolunteerController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Volunteer Id</param>
        /// <returns></returns>
        [VolunteerAuthorization]
        [HttpGet]
        [ActionName("find-volunteer-references")]
        public List<VolunteerReferenceResponse> FindVolunteerReferences()
        {
            var queryable = from cscVolunteerReference in _ctx.csc_VolunteerReferenceSet
                orderby cscVolunteerReference.CreatedOn descending
                where cscVolunteerReference.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteerReference;

            return queryable.ToList().ConvertAll(volunteerReferenceEntity =>
                _mapper.Map<VolunteerReferenceResponse>(volunteerReferenceEntity));
        }
    }
}