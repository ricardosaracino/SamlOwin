using System;
using System.ComponentModel.DataAnnotations;

namespace SamlOwin.Models
{
    public class ScheduleCalendarRequestParams
    {
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
    }
}