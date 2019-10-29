using System;

namespace SamlOwin.Models
{    
    public sealed class Volunteer
    {
        public Guid Id { get; set; }
        
        public bool? CanApplyCac { get; set; }

        public bool? CanApplyCsc { get; set; }
        
        public bool? CanApplyReac { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string ReferenceNumber { get; set; }
        
        public DateTime? EmailVerifiedOn { get; set; }
    }
}