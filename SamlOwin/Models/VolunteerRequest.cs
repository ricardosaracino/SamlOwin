using System;
using System.ComponentModel.DataAnnotations;

namespace SamlOwin.Models
{
    public sealed class VolunteerRequest
    {
        public Guid Id { get; set; }

        [Required] [MaxLength(100)] public string FirstName { get; set; }

        [Required] [MaxLength(100)] public string HomeAddressCity { get; set; }

        [Required] public ReferenceResponse HomeAddressCountry { get; set; }

        [Required] [MaxLength(100)] public string HomeAddressLine1 { get; set; }

        [MaxLength(100)] public string HomeAddressLine2 { get; set; }

        [Required] [MaxLength(100)] public string HomeAddressPostalCode { get; set; }

        [Required] public ReferenceResponse HomeAddressProvinceOrState { get; set; }

        [Required] [MaxLength(100)] public string HomeTelephone { get; set; }

        [Required] [MaxLength(100)] public string LastName { get; set; }

        [MaxLength(100)] public string MailingAddressCity { get; set; }

        public ReferenceResponse MailingAddressCountry { get; set; }

        [MaxLength(100)] public string MailingAddressLine1 { get; set; }

        [MaxLength(100)] public string MailingAddressLine2 { get; set; }

        [MaxLength(100)] public string MailingAddressPostalCode { get; set; }

        public ReferenceResponse MailingAddressProvinceOrState { get; set; }

        [MaxLength(100)] public string MiddleName { get; set; }
    }
}