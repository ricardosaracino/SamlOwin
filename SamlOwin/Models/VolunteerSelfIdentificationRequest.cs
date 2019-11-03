using System;
using System.Collections.Generic;

namespace SamlOwin.Models
{
    public class VolunteerSelfIdentificationRequest
    {
        public Guid Id { get; set; }

        public bool AgreeSelfIdentification { get; set; }

        public List<int> AboriginalTypes { get; set; }

        public List<int> CultureTypes { get; set; }

        public List<int> DisabilityTypes { get; set; }

        public List<int> MinorityTypes { get; set; }
    }
}