using System;

namespace SamlOwin.Models
{
    public sealed class VolunteerResponse
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string HomeAddressCity { get; set; }

        public ReferenceResponse HomeAddressCountry { get; set; }

        public string HomeAddressLine1 { get; set; }

        public string HomeAddressLine2 { get; set; }

        public string HomeAddressPostalCode { get; set; }

        public ReferenceResponse HomeAddressProvinceOrState { get; set; }

        public string HomeTelephone { get; set; }

        public string LastName { get; set; }

        public string MailingAddressCity { get; set; }

        public ReferenceResponse MailingAddressCountry { get; set; }

        public string MailingAddressLine1 { get; set; }

        public string MailingAddressLine2 { get; set; }

        public string MailingAddressPostalCode { get; set; }

        public ReferenceResponse MailingAddressProvinceOrState { get; set; }

        public string MiddleName { get; set; }

        public string ReferenceNumber { get; set; }
    }
}