using System;

namespace SamlOwin.Models
{    
    public sealed class Volunteer
    {
        public Guid Id { get; set; }
        
        public bool? CanApplyCac { get; set; }       
        
        public bool? CanApplyReac { get; set; }        
        
        public bool? CanApplyCsc { get; set; }
        
        public DateTime? EmailVerifiedOn { get; set; }
    }
}