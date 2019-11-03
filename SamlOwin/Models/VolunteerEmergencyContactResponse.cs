using System;

namespace SamlOwin.Models
{
    public class VolunteerEmergencyContactResponse
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string AddressLine1 { get; set; }

        public string PrimaryTelephone { get; set; }

        public string SecondaryTelephone { get; set; }

        public ReferenceResponse Volunteer { get; set; }
    }
}