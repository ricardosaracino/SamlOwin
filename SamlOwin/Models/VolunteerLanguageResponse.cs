using System;

namespace SamlOwin.Models
{
    public class VolunteerLanguageResponse
    {
        public Guid Id { get; set; }

        public bool? CanRead { get; set; }
        public bool? CanSpeak { get; set; }
        public bool? CanWrite { get; set; }
        public bool? WillInterpret { get; set; }
        public bool? WillTranslate { get; set; }

        public ReferenceResponse Language { get; set; }
        public ReferenceResponse Volunteer { get; set; }
    }
}