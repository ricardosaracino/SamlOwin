using System;

namespace SamlOwin.Models
{
    public class VolunteerSelfIdentificationResponse
    {
        public Guid Id { get; set; }

        public bool AgreeSelfIdentification { get; set; }

        public int[] AboriginalTypes { get; set; }

        public int[] CultureTypes { get; set; }

        public int[] DisabilityTypes { get; set; }

        public int[] MinorityTypes { get; set; }
    }
}