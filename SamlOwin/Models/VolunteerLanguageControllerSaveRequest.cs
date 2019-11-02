using System.Collections.Generic;

namespace SamlOwin.Models
{
    public class VolunteerLanguageControllerSaveRequest
    {
        public IEnumerable<VolunteerLanguageRequest> VolunteerLanguages { get; set; }
    }
}