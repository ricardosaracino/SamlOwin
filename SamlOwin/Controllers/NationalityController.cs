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
    [RoutePrefix("api/nationalities")]
    public class NationalityController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        private const string CacheKey = "NationalityController.NationalityResponse";

        public NationalityController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Nationalities
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<NationalityResponse>> FindAll()
        {
            var nationalityResponses =
                (List<NationalityResponse>) MemoryCache.Default.Get(CacheKey);

            if (nationalityResponses == null)
            {
                var queryable = from nationalityEntity in _ctx.csc_NationalitySet
                    select nationalityEntity;

                nationalityResponses =
                    queryable.ToList().ConvertAll(entity => _mapper.Map<NationalityResponse>(entity));

                MemoryCache.Default.Set(new CacheItem(CacheKey, nationalityResponses), new CacheItemPolicy());
            }

            return new WebApiSuccessResponse<List<NationalityResponse>>
            {
                Data = nationalityResponses
            };
        }
    }
}