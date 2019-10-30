﻿using System;

namespace SamlOwin.Models
{    
    public sealed class Volunteer
    {
        public Guid Id { get; set; }
        
        public bool? CanApplyCac { get; set; }

        public bool? CanApplyCsc { get; set; }
        
        public bool? CanApplyReac { get; set; }

        public string FirstName { get; set; }

        public string HomeAddressCity { get; set; }
        
        public Reference HomeAddressCountry { get; set; }
        
        public string HomeAddressLine1 { get; set; }
        
        public string HomeAddressLine2 { get; set; }
        
        public string HomeAddressPostalCode { get; set; }
        
        public Reference HomeAddressProvinceOrState { get; set; }
        
        public string HomeTelephone { get; set; }

        public string LastName { get; set; }
        
        public string MailingAddressCity { get; set; }
        
        public Reference MailingAddressCountry { get; set; }
        
        public string MailingAddressLine1 { get; set; }
        
        public string MailingAddressLine2 { get; set; }
        
        public string MailingAddressPostalCode { get; set; }
        
        public Reference MailingAddressProvinceOrState { get; set; }
        
        public string MiddleName { get; set; }

        public string ReferenceNumber { get; set; }
        
        public DateTime? EmailVerifiedOn { get; set; }
    }
}