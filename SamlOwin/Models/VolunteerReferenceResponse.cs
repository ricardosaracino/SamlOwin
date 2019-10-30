using System;

namespace SamlOwin.Models
{
    public class VolunteerReferenceResponse
    {
        public Guid Id { get; set; }

        public int? FirstContactMethod { get; set; }

        public string FirstContactValue { get; set; }

        public string FullName { get; set; }
        
        public string Occupation { get; set; }

        public int? ReferenceType { get; set; }

        public int? SecondContactMethod { get; set; }

        public string SecondContactValue { get; set; }
        
        public string Title { get; set; }

        public ObjectResponse Volunteer { get; set; }
    }
}