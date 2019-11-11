using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerApplicationRequestToVolunteerApplicationEntity : Profile
    {
        public VolunteerApplicationRequestToVolunteerApplicationEntity()
        {
            CreateMap<VolunteerApplicationRequest, csc_VolunteerApplication>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StatusCodeEnum,
                    opt => opt.MapFrom(s => s.ApplicationStatus))
                .ForMember(dest => dest.csc_ActivityApplyingForEnum,
                    opt => opt.MapFrom(s => s.ApplicationType))
                .ForMember(dest => dest.csc_AgreeAgeOfMajority,
                    opt => opt.MapFrom(s => s.AgreeAgeOfMajority))
                .ForMember(dest => dest.csc_AgreeCanadianCitizen,
                    opt => opt.MapFrom(s => s.AgreeCanadianCitizen))
                .ForMember(dest => dest.csc_OnInmateVisitingList,
                    opt => opt.MapFrom(s => s.OnInmateVisitingList))
                .ForMember(dest => dest.csc_PersonallyKnowOffenders,
                    opt => opt.MapFrom(s => s.PersonallyKnowOffenders))
                .ForMember(dest => dest.csc_ConvictedNotPardoned,
                    opt => opt.MapFrom(s => s.ConvictedNotPardoned))
                .ForMember(dest => dest.csc_OutstandingCharges,
                    opt => opt.MapFrom(s => s.OutstandingCharges))
                .ForMember(dest => dest.csc_GeneralActivityTypes,
                    opt =>
                    {
                        // skip if creating empty .. XRM sdk kept throwing exceptions
                        opt.PreCondition(s => (s.Id != null || s.GeneralActivities?.Length > 0));
                        opt.MapFrom((s, d) =>
                            new OptionSetValueCollection(
                                s.GeneralActivities.ToList().ConvertAll(v => new OptionSetValue(v))));
                    })
                .ForMember(dest => dest.csc_ActivityChoiceReason,
                    opt => opt.MapFrom(s => s.ActivityChoiceReason))
                .ForMember(dest => dest.csc_DescribeAffiliationsWithGroups,
                    opt => opt.MapFrom(s => s.AffiliationsWithGroups))
                .ForMember(dest => dest.csc_CurrentlyVolunteeringOrApplyingWith,
                    opt => opt.MapFrom(s => s.CurrentlyVolunteeringApplyingWith))
                .ForMember(dest => dest.csc_SkillOrExpertise,
                    opt => opt.MapFrom(s => s.SkillOrExpertise))
                .ForMember(dest => dest.csc_HasPreviousCommitteeExperience,
                    opt => opt.MapFrom(s => s.CommitteeExperience))
                .ForMember(dest => dest.csc_PreviousWorkVolunteerExperience,
                    opt => opt.MapFrom(s => s.PreviousWorkVolunteerExperience))
                .ForMember(dest => dest.csc_MemberOfUnderRepresentedGroup,
                    opt => opt.MapFrom(s => s.MemberOfUnderRepresentedGroup))
                .ForMember(dest => dest.csc_HearAboutDetails,
                    opt => opt.MapFrom(s => s.HearAboutVolunteering))
                .ForMember(dest => dest.csc_ReasonForVolunteering,
                    opt => opt.MapFrom(s => s.ReasonForVolunteering))
                .ForMember(dest => dest.csc_AdditionalInformation,
                    opt => opt.MapFrom(s => s.AdditionalVolunteeringInfo))
                .ForMember(dest => dest.csc_AgreeAcknowledge,
                    opt => opt.MapFrom(s => s.AgreementAcknowledged))
                ;
        }
    }
}