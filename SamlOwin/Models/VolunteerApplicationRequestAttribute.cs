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

            if (applicationStatus == _applicationStatus && !base.IsValid(value))
            {
                return new ValidationResult(
                    $"The {validationContext.DisplayName} field is required on Application Status {_applicationStatus}.");
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

        public RequiredApplicationStatusSubmittedApplicationTypeAttribute(int applicationType) : base(
            (int) csc_VolunteerApplication_StatusCode.Submitted)
        {
            _applicationType = applicationType;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var applicationType =
                (validationContext.ObjectInstance as VolunteerApplicationRequest)?.ApplicationType;

            if (applicationType == _applicationType && !base.IsValid(value))
            {
                return new ValidationResult(
                    $"The {validationContext.DisplayName} field is required on Application Type {_applicationType}.");
            }

            return ValidationResult.Success;
        }
    }
    
    public class RequiredApplicationStatusSubmittedApplicationTypeCacAttribute : RequiredApplicationStatusAttribute
    {
        public RequiredApplicationStatusSubmittedApplicationTypeCacAttribute() : base((int) csc_VolunteerApplicationType.CitizensAdvisoryCommittee_CACMembership)
        {
        }
    }
    
    public class RequiredApplicationStatusSubmittedApplicationTypeCscAttribute : RequiredApplicationStatusAttribute
    {
        public RequiredApplicationStatusSubmittedApplicationTypeCscAttribute() : base((int) csc_VolunteerApplicationType.CSCVolunteer)
        {
        }
    }
    
    public class RequiredApplicationStatusSubmittedApplicationTypeReacAttribute : RequiredApplicationStatusAttribute
    {
        public RequiredApplicationStatusSubmittedApplicationTypeReacAttribute() : base((int) csc_VolunteerApplicationType.RegionalEthnoCulturalAdvisoryCommitee_REACMembership)
        {
        }
    }
}