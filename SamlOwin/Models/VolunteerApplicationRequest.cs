using System;
using System.ComponentModel.DataAnnotations;

namespace SamlOwin.Models
{
    public class VolunteerApplicationRequest
    {
        public Guid? Id { get; set; }
        
        [Required]
        public int ApplicationStatus { get; set; }
        
        [Required]
        public int ApplicationType { get; set; }
        
        //
        [RequiredApplicationStatusSubmitted]
        public bool? AgreeAgeOfMajority { get; set; }
        
        [RequiredApplicationStatusSubmitted]
        public bool? AgreeCanadianCitizen { get; set; }
        
        //
        [RequiredApplicationStatusSubmitted]
        public bool? OnInmateVisitingList { get; set; }
        
        [RequiredApplicationStatusSubmitted]        
        public bool? PersonallyKnowOffenders { get; set; }
        
        [RequiredApplicationStatusSubmitted]
        public bool? ConvictedNotPardoned { get; set; }
        
        [RequiredApplicationStatusSubmitted]        
        public bool? OutstandingCharges { get; set; }
        
        
        
        [RequiredApplicationStatusSubmittedApplicationTypeCac]
        public int[] GeneralActivities { get; set; }

        
        //
        public string HearAboutVolunteering { get; set; }
        
        public string ReasonForVolunteering { get; set; }
        
        public string AdditionalVolunteeringInfo { get; set; }
        
        
        //
        [RequiredApplicationStatusSubmitted]        
        public bool? AgreementAcknowledged { get; set; }
    }
}