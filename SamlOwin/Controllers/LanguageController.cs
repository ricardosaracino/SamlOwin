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
    [Authorize]
    [RoutePrefix("api/languages")]
    public class LanguageController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;

        public LanguageController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Languages
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<LanguageResponse>> FindAll()
        {
            var languageResponses =
                (List<LanguageResponse>) MemoryCache.Default.Get("LanguageController.LanguageResponse");

            if (languageResponses == null)
            {
                var queryable = from cscLanguageEntity in _ctx.csc_LanguageSet
                    select cscLanguageEntity;

                languageResponses = queryable.ToList().ConvertAll(entity => _mapper.Map<LanguageResponse>(entity));

                MemoryCache.Default.Set(new CacheItem("LanguageController.LanguageResponse", languageResponses),
                    new CacheItemPolicy());
            }

            return new WebApiSuccessResponse<List<LanguageResponse>>
            {
                Data = languageResponses
            };
        }
    }
}