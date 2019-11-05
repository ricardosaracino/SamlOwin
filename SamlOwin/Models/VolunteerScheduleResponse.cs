using System;

namespace SamlOwin.Models
{
    public class VolunteerScheduleResponse
    {
        public Guid Id { get; set; }

        public DateTime ShiftStart { get; set; }

        public DateTime ShiftEnd { get; set; }

        public LocationResponse Location { get; set; }
        
        public ScheduledActivityTypeResponse ScheduledActivityType { get; set; }

        public ReferenceResponse Volunteer { get; set; }
    }
}