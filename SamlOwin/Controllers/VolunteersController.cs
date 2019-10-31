using System;
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
    public class VolunteersController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public VolunteersController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }
        
        [VolunteerAuthorization]
        [HttpGet]
        [ActionName("find-one")]
        public ApiResponse<VolunteersControllerFindOneResponse> FindOne()
        {
            var queryable = from cscVolunteer in _ctx.csc_VolunteerSet
                where cscVolunteer.Id.Equals(User.Identity.GetVolunteerId())
                select cscVolunteer;
            
            return new ApiSuccessResponse<VolunteersControllerFindOneResponse>
            {
                Data = new VolunteersControllerFindOneResponse
                {
                    Volunteer = _mapper.Map<Volunteer>(queryable.First())
                }
            };
        }
    }
}