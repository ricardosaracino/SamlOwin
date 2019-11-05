using System;

namespace SamlOwin.Models
{
    public class VolunteerApplicationResponse
    {
        public Guid Id { get; set; }

        public string ReferenceNumber { get; set; }
        
        /// <summary>
        /// Draft, Submitted
        /// </summary>
        public int Status { get; set; }

        public int ActivityApplyingFor { get; set; }

        public DateTime CancelledOn { get; set; }
        
        public string CancelledComment { get; set; }
        
        public DateTime CreatedOn { get; set; }
    }
}