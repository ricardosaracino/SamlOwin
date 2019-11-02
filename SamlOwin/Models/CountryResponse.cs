using System;
using System.Collections.Generic;

namespace SamlOwin.Models
{
    public class CountryResponse
    {
        public Guid Id { get; set; }

        public string EnLabel { get; set; }

        public string FrLabel { get; set; }

        public List<ProvinceOrStateResponse> ProvinceStates { get; set; }
    }
}