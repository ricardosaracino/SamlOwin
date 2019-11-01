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
        [HttpGet, Route("find-all")]
        public ApiResponse<VolunteerReferencesControllerFindAllResponse> FindAll()
        {
            var queryable = from cscVolunteerReference in _ctx.csc_VolunteerReferenceSet
                orderby cscVolunteerReference.CreatedOn descending
                where cscVolunteerReference.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteerReference;

            return new ApiResponse<VolunteerReferencesControllerFindAllResponse>
            {
                Data = new VolunteerReferencesControllerFindAllResponse
                {
                    VolunteerReferences = queryable.ToList()
                        .ConvertAll(entity => _mapper.Map<VolunteerReferenceResponse>(entity))
                }
            };
        }
    }
}