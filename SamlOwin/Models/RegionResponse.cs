using System;
using System.Collections.Generic;

namespace SamlOwin.Models
{
    public class RegionResponse
    {
        public Guid Id { get; set; }

        public string EnLabel { get; set; }

        public string FrLabel { get; set; }
        
        public List<LocationResponse> Locations { get; set; }
    }
}