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
        
        public Reference Language { get; set; }
        public Reference Volunteer { get; set; }
    }
}