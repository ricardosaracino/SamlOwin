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
    [RoutePrefix("api/regions")]
    public class RegionController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        private const string CacheKey = "RegionController.RegionResponse";

        public RegionController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Regions
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<RegionResponse>> FindAll()
        {
            var regionResponses =
                (List<RegionResponse>) MemoryCache.Default.Get(CacheKey);

            if (regionResponses == null)
            {
                var regionQueryable = from regionEntity in _ctx.csc_RegionSet
                    select regionEntity;

                regionResponses = regionQueryable.ToList().ConvertAll(entity => _mapper.Map<RegionResponse>(entity));

               var psQueryable = from locationEntity in _ctx.csc_LocationSet
                    select locationEntity;

                psQueryable.ToList().ForEach(psEntity =>
                {
                    var regionResponse = regionResponses.Single(c => c.Id == psEntity.csc_Region.Id);

                    if (regionResponse.Locations == null)
                    {
                        regionResponse.Locations = new List<LocationResponse>();
                    }

                    regionResponse.Locations.Add(_mapper.Map<LocationResponse>(psEntity));
                });
               
               MemoryCache.Default.Set(new CacheItem(CacheKey, regionResponses), new CacheItemPolicy());
            }

            return new WebApiSuccessResponse<List<RegionResponse>>
            {
                Data = regionResponses
            };
        }
    }
}