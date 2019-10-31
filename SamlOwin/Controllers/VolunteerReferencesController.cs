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
    public class VolunteerReferencesController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerReferencesController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        [VolunteerAuthorization]
        [HttpGet]
        [ActionName("find-all")]
        public ApiResponse<VolunteerReferencesControllerFindAllResponse> FindAll()
        {
            var queryable = from cscVolunteerReference in _ctx.csc_VolunteerReferenceSet
                orderby cscVolunteerReference.CreatedOn descending
                where cscVolunteerReference.csc_Volunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteerReference;

            return new ApiSuccessResponse<VolunteerReferencesControllerFindAllResponse>
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