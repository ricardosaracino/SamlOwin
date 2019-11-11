using System;
using System.ComponentModel.DataAnnotations;

namespace SamlOwin.Models
{
    public sealed class VolunteerRequest
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string MaidenName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string SecondaryEmail { get; set; }

        [Required]
        public ReferenceRequest Citizenship { get; set; }

        [Required]
        public int PreferredLanguage { get; set; }

        [Required]
        public int Gender { get; set; }

        [StringLength(100)]
        public string GenderOther { get; set; }

        [Required]
        [StringLength(100)]
        public string HomeAddressCity { get; set; }

        [Required]
        public ReferenceRequest HomeAddressCountry { get; set; }

        [Required]
        [StringLength(100)]
        public string HomeAddressLine1 { get; set; }

        [StringLength(100)]
        public string HomeAddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string HomeAddressPostalCode { get; set; }

        public ReferenceRequest HomeAddressProvinceOrState { get; set; }

        [Required]
        public bool MailingAddressSameAsHomeAddress { get; set; }

        [StringLength(100)]
        public string MailingAddressCity { get; set; }

        public ReferenceRequest MailingAddressCountry { get; set; }

        [StringLength(100)]
        public string MailingAddressLine1 { get; set; }

        [StringLength(100)]
        public string MailingAddressLine2 { get; set; }

        [StringLength(100)]
        public string MailingAddressPostalCode { get; set; }

        public ReferenceRequest MailingAddressProvinceOrState { get; set; }

        [Required]
        [StringLength(100)]
        public string HomeTelephone { get; set; }

        [StringLength(100)]
        public string BusinessTelephone { get; set; }

        [StringLength(100)]
        public string BusinessTelephoneExt { get; set; }

        [StringLength(100)]
        public string OtherTelephone { get; set; }

        [StringLength(100)]
        public string OtherTelephoneExt { get; set; }

        public bool AgreeContactedWork { get; set; }

        [StringLength(2000)]
        public string Availability { get; set; }
    }
}