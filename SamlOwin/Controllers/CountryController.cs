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
    [RoutePrefix("api/countries")]
    public class CountryController : ApiController
    {
        private readonly CrmServiceContext _ctx;
        private readonly Mapper _mapper;
        
        private const string CacheKey = "CountryController.CountryResponse";

        public CountryController()
        {
            _ctx = HttpContext.Current.GetOwinContext().Get<CrmServiceContext>();
            _mapper = AutoMapperProvider.GetMapper();
        }

        /// <summary>
        /// Finds all Countries with Provinces or States attached
        /// </summary>
        [HttpGet, Route("")]
        public WebApiSuccessResponse<List<CountryResponse>> FindAll()
        {
            var countryResponses =
                (List<CountryResponse>) MemoryCache.Default.Get(CacheKey);

            if (countryResponses == null)
            {
                var countryQueryable = from countryEntity in _ctx.csc_CountrySet
                    select countryEntity;

                countryResponses = countryQueryable.ToList().ConvertAll(entity => _mapper.Map<CountryResponse>(entity));

                var psQueryable = from provinceStateEntity in _ctx.csc_ProvinceOrStateSet
                    select provinceStateEntity;

                psQueryable.ToList().ForEach(psEntity =>
                {
                    var countryResponse = countryResponses.Single(c => c.Id == psEntity.csc_Country.Id);

                    if (countryResponse.ProvinceStates == null)
                    {
                        countryResponse.ProvinceStates = new List<ProvinceOrStateResponse>();
                    }

                    countryResponse.ProvinceStates.Add(_mapper.Map<ProvinceOrStateResponse>(psEntity));
                });

                MemoryCache.Default.Set(new CacheItem(CacheKey, countryResponses), new CacheItemPolicy());
            }

            return new WebApiSuccessResponse<List<CountryResponse>>
            {
                Data = countryResponses
            };
        }
    }
}