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
    public class VolunteerLanguagesController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteerLanguagesController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        [VolunteerAuthorization]
        [HttpGet]
        [ActionName("find-all")]
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
    }
}