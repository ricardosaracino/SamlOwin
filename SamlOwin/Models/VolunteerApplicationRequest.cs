﻿using System;
using System.Collections.Generic;
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

        //
        // [MinLength(2)] only apply if Required
        [RequiredApplicationStatusSubmittedApplicationTypeCsc]
        public int[] GeneralActivities { get; set; }
        
        // [StringLength(2000, MinimumLength = 2)]
        // [RequiredApplicationStatusSubmittedApplicationTypeCsc]
        [StringLength(2000)]
        public string ActivityChoiceReason { get; set; }

        [StringLength(2000)]
        public string AffiliationsWithGroups { get; set; }

        [StringLength(2000)]
        public string CurrentlyVolunteeringApplyingWith { get; set; }

        [StringLength(2000)]
        public string SkillOrExpertise { get; set; }

        [StringLength(2000)]
        public string CommitteeExperience { get; set; }

        [StringLength(2000)]
        public string PreviousWorkVolunteerExperience { get; set; }

        [StringLength(2000)]
        public string MemberOfUnderRepresentedGroup { get; set; }

        [StringLength(2000)]
        public string HearAboutVolunteering { get; set; }

        [StringLength(2000)]
        public string ReasonForVolunteering { get; set; }

        [StringLength(2000)]
        public string AdditionalVolunteeringInfo { get; set; }

        //
        [RequiredApplicationStatusSubmitted]
        public bool? AgreementAcknowledged { get; set; }
    }
}