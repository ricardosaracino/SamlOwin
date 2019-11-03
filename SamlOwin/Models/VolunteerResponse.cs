using System;

namespace SamlOwin.Models
{
    public sealed class VolunteerResponse
    {
        public Guid Id { get; set; }

        public string ReferenceNumber { get; set; }


        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string MaidenName { get; set; }
        
        public string Email { get; set; }

        public string SecondaryEmail { get; set; }

        public ReferenceResponse Citizenship { get; set; }

        public int PreferredLanguage { get; set; }
        
        public int? Gender { get; set; }

        public string GenderOther { get; set; }

        public string HomeAddressCity { get; set; }

        public ReferenceResponse HomeAddressCountry { get; set; }

        public string HomeAddressLine1 { get; set; }

        public string HomeAddressLine2 { get; set; }

        public string HomeAddressPostalCode { get; set; }

        public ReferenceResponse HomeAddressProvinceOrState { get; set; }
        
        public bool MailingAddressSameAsHomeAddress { get; set; }

        public string MailingAddressCity { get; set; }

        public ReferenceResponse MailingAddressCountry { get; set; }

        public string MailingAddressLine1 { get; set; }

        public string MailingAddressLine2 { get; set; }

        public string MailingAddressPostalCode { get; set; }

        public ReferenceResponse MailingAddressProvinceOrState { get; set; }

        public string HomeTelephone { get; set; }

        public string BusinessTelephone { get; set; }

        public string BusinessTelephoneExt { get; set; }

        public string OtherTelephone { get; set; }

        public string OtherTelephoneExt { get; set; }
        
        public bool AgreeContactedWork { get; set; }

        public string Availability { get; set; }
    }
}