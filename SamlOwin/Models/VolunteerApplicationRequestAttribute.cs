using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CrmEarlyBound;

namespace SamlOwin.Models
{
    public class RequiredApplicationStatusAttribute : RequiredAttribute
    {
        private readonly int _applicationStatus;

        protected RequiredApplicationStatusAttribute(int applicationStatus)
        {
            _applicationStatus = applicationStatus;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var applicationStatus =
                (validationContext.ObjectInstance as VolunteerApplicationRequest)?.ApplicationStatus;

            var isValid = base.IsValid(value, validationContext);
            
            if (applicationStatus == _applicationStatus && isValid != ValidationResult.Success)
            {
                return isValid;
            }

            return ValidationResult.Success;
        }
    }

    public class RequiredApplicationStatusSubmittedAttribute : RequiredApplicationStatusAttribute
    {
        public RequiredApplicationStatusSubmittedAttribute() : base((int) csc_VolunteerApplication_StatusCode.Submitted)
        {
        }
    }

    public class RequiredApplicationStatusSubmittedApplicationTypeAttribute : RequiredApplicationStatusAttribute
    {
        private readonly int _applicationType;

        protected RequiredApplicationStatusSubmittedApplicationTypeAttribute(int applicationType) : base(
            (int) csc_VolunteerApplication_StatusCode.Submitted)
        {
            _applicationType = applicationType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var applicationType =
                (validationContext.ObjectInstance as VolunteerApplicationRequest)?.ApplicationType;

            var isValid = base.IsValid(value, validationContext);

            if (applicationType == _applicationType && isValid != ValidationResult.Success)
            {
                return isValid;
            }

            return ValidationResult.Success;
        }
    }

    public class
        RequiredApplicationStatusSubmittedApplicationTypeCacAttribute :
            RequiredApplicationStatusSubmittedApplicationTypeAttribute
    {
        public RequiredApplicationStatusSubmittedApplicationTypeCacAttribute() : base(
            (int) csc_VolunteerApplicationType.CitizensAdvisoryCommittee_CACMembership)
        {
        }
    }

    public class
        RequiredApplicationStatusSubmittedApplicationTypeCscAttribute :
            RequiredApplicationStatusSubmittedApplicationTypeAttribute
    {
        public RequiredApplicationStatusSubmittedApplicationTypeCscAttribute() : base(
            (int) csc_VolunteerApplicationType.CSCVolunteer)
        {
        }
    }

    public class
        RequiredApplicationStatusSubmittedApplicationTypeReacAttribute :
            RequiredApplicationStatusSubmittedApplicationTypeAttribute
    {
        public RequiredApplicationStatusSubmittedApplicationTypeReacAttribute() : base(
            (int) csc_VolunteerApplicationType.RegionalEthnoCulturalAdvisoryCommitee_REACMembership)
        {
        }
    }
}