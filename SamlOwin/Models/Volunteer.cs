using System;
using XrmFramework.Attributes;

namespace SamlOwin.Models
{    
    [Entity("csc_volunteer")]
    public class Volunteer
    {
        [Id] 
        [Column("csc_volunteerid")] 
        public Guid Id { get; set; }

        [Name] 
        [Column("csc_name")] 
        public string UserName { get; set; }
        
        [Column("csc_emailverifiedon")] 
        public DateTime EmailVerifiedOn { get; set; }
        
        [Column("csc_canapplycac")] 
        public bool CanApplyCac { get; set; }       
        
        [Column("csc_canapplyreac")] 
        public bool CanApplyReac { get; set; }        
        
        [Column("csc_canapplygeneral")] 
        public bool CanApplyCsc { get; set; }
    }
}