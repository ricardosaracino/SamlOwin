using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerApplicationEntityToVolunteerApplicationResponse : Profile
    {
        public VolunteerApplicationEntityToVolunteerApplicationResponse()
        {
            CreateMap<csc_VolunteerApplication, VolunteerApplicationResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(s => s.csc_VolunteerApplicationId))
                .ForMember(dest => dest.ReferenceNumber,
                    opt => opt.MapFrom(s => s.csc_ReferenceNumber))
                .ForMember(dest => dest.ApplicationStatus,
                    opt => opt.MapFrom(s => s.StatusCodeEnum))
                .ForMember(dest => dest.ApplicationType,
                    opt => opt.MapFrom(s => s.csc_ActivityApplyingForEnum))
                .ForMember(dest => dest.AgreeAgeOfMajority,
                    opt => opt.MapFrom(s => s.csc_AgreeAgeOfMajority))
                .ForMember(dest => dest.AgreeCanadianCitizen,
                    opt => opt.MapFrom(s => s.csc_AgreeCanadianCitizen))
                .ForMember(dest => dest.OnInmateVisitingList,
                    opt => opt.MapFrom(s => s.csc_OnInmateVisitingList))
                .ForMember(dest => dest.PersonallyKnowOffenders,
                    opt => opt.MapFrom(s => s.csc_PersonallyKnowOffenders))
                .ForMember(dest => dest.ConvictedNotPardoned,
                    opt => opt.MapFrom(s => s.csc_ConvictedNotPardoned))
                .ForMember(dest => dest.OutstandingCharges,
                    opt => opt.MapFrom(s => s.csc_OutstandingCharges))
                .ForMember(dest => dest.Region,
                    opt => opt.MapFrom(s => s.csc_RegionId))  
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(s => s.csc_LocationId)) 
                .ForMember(dest => dest.GeneralActivities,
                    opt => opt.MapFrom(s => s.csc_GeneralActivityTypesEnum))                
                .ForMember(dest => dest.ActivityChoiceReason,
                    opt => opt.MapFrom(s => s.csc_ActivityChoiceReason))
                .ForMember(dest => dest.AffiliationsWithGroups,
                    opt => opt.MapFrom(s => s.csc_DescribeAffiliationsWithGroups))
                .ForMember(dest => dest.CurrentlyVolunteeringApplyingWith,
                    opt => opt.MapFrom(s => s.csc_CurrentlyVolunteeringOrApplyingWith))
                .ForMember(dest => dest.SkillOrExpertise,
                    opt => opt.MapFrom(s => s.csc_SkillOrExpertise))
                .ForMember(dest => dest.CommitteeExperience,
                    opt => opt.MapFrom(s => s.csc_HasPreviousCommitteeExperience))
                .ForMember(dest => dest.PreviousWorkVolunteerExperience,
                    opt => opt.MapFrom(s => s.csc_PreviousWorkVolunteerExperience))
                .ForMember(dest => dest.MemberOfUnderRepresentedGroup,
                    opt => opt.MapFrom(s => s.csc_MemberOfUnderRepresentedGroup))
                .ForMember(dest => dest.HearAboutVolunteering,
                    opt => opt.MapFrom(s => s.csc_HearAboutDetails))
                .ForMember(dest => dest.ReasonForVolunteering,
                    opt => opt.MapFrom(s => s.csc_ReasonForVolunteering))
                .ForMember(dest => dest.AdditionalVolunteeringInfo,
                    opt => opt.MapFrom(s => s.csc_AdditionalInformation))
                .ForMember(dest => dest.AgreementAcknowledged,
                    opt => opt.MapFrom(s => s.csc_AgreeAcknowledge))
                .ForMember(dest => dest.DecisionStatus,
                    opt => opt.MapFrom(s => s.csc_DecisionEnum))
                .ForMember(dest => dest.DecisionOn,
                    opt => opt.MapFrom(s => s.csc_DecisionDate))
                .ForMember(dest => dest.CancelledComment,
                    opt => opt.MapFrom(s => s.csc_CancelledComment))
                .ForMember(dest => dest.CancelledOn,
                    opt => opt.MapFrom(s => s.csc_CancelledDate))
                .ForMember(dest => dest.CreatedOn,
                    opt => opt.MapFrom(s => s.CreatedOn))
                ;
        }
    }
}