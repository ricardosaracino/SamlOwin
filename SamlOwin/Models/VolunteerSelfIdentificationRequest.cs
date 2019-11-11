using System;
using System.Collections.Generic;

namespace SamlOwin.Models
{
    public class VolunteerSelfIdentificationRequest
    {
        public Guid Id { get; set; }

        public bool AgreeSelfIdentification { get; set; }

        public int[] AboriginalTypes { get; set; }

        public int[] CultureTypes { get; set; }

        public int[] DisabilityTypes { get; set; }

        public int[] MinorityTypes { get; set; }
    }
}