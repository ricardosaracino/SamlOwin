using System;

namespace SamlOwin.Models
{
    public class VolunteerApplicationResponse
    {
        public Guid Id { get; set; }

        //
        public string ReferenceNumber { get; set; }

        public int? DecisionStatus { get; set; }

        public DateTime? DecisionOn { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancelledComment { get; set; }

        public DateTime CreatedOn { get; set; }

        //
        public int ApplicationStatus { get; set; }

        public int ApplicationType { get; set; }


        //
        public bool? AgreeAgeOfMajority { get; set; }

        public bool? AgreeCanadianCitizen { get; set; }


        //
        public bool? OnInmateVisitingList { get; set; }

        public bool? PersonallyKnowOffenders { get; set; }

        public bool? ConvictedNotPardoned { get; set; }

        public bool? OutstandingCharges { get; set; }


        //
        public string HearAboutVolunteering { get; set; }

        public string ReasonForVolunteering { get; set; }

        public string AdditionalVolunteeringInfo { get; set; }


        //
        public bool? AgreementAcknowledged { get; set; }
    }
}