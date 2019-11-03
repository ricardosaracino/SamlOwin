using System;
using System.ComponentModel.DataAnnotations;

namespace SamlOwin.Models
{
    public class VolunteerLanguageRequest
    {
        public Guid? Id { get; set; }

        public bool? CanRead { get; set; }
        public bool? CanSpeak { get; set; }
        public bool? CanWrite { get; set; }
        public bool? WillInterpret { get; set; }
        public bool? WillTranslate { get; set; }

        [Required] public ReferenceRequest Language { get; set; }
        
        public ReferenceRequest Volunteer { get; set; }
    }
}