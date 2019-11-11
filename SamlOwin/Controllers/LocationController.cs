using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.AspNet.Identity.Owin;
using SamlOwin.Models;
using SamlOwin.Providers;

namespace SamlOwin.Controllers
{
    /// <summary>
    /// Authorize Permission
    /// </summary>
    [Authorize]
    [RoutePrefix("api/locations")]
    public class LocationController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public LocationController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Locations
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<LocationResponse>> FindAll()
        {
            var locationResponses =
                (List<LocationResponse>) MemoryCache.Default.Get("LocationController.LocationResponse");

            if (locationResponses == null)
            {
                var queryable = from cscLocationEntity in _ctx.csc_LocationSet
                    select cscLocationEntity;

                locationResponses = queryable.ToList().ConvertAll(entity => _mapper.Map<LocationResponse>(entity));

                MemoryCache.Default.Set(new CacheItem("LocationController.LocationResponse", locationResponses),
                    new CacheItemPolicy());
            }

            return new WebApiSuccessResponse<List<LocationResponse>>
            {
                Data = locationResponses
            };
        }
    }
}