using System;
using System.ComponentModel.DataAnnotations;

namespace SamlOwin.Models
{
    public class VolunteerReferenceRequest
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public int ReferenceType { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Occupation { get; set; }

        [Required]
        public int FirstContactMethod { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstContactValue { get; set; }

        public int? SecondContactMethod { get; set; }

        [MaxLength(100)]
        public string SecondContactValue { get; set; }

        public ReferenceRequest Volunteer { get; set; }
    }
}