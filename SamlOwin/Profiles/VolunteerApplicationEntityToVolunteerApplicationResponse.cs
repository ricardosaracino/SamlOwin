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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerApplicationId))
                .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(s => s.csc_ReferenceNumber))
                .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(s => s.StatusCodeEnum))
                .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(s => s.csc_ActivityApplyingForEnum))
                
                .ForMember(dest => dest.AgreeAgeOfMajority, opt => opt.MapFrom(s => s.csc_AgreeAgeOfMajority))
                .ForMember(dest => dest.AgreeCanadianCitizen, opt => opt.MapFrom(s => s.csc_AgreeCanadianCitizen))

                .ForMember(dest => dest.OnInmateVisitingList, opt => opt.MapFrom(s => s.csc_OnInmateVisitingList))
                .ForMember(dest => dest.PersonallyKnowOffenders, opt => opt.MapFrom(s => s.csc_PersonallyKnowOffenders))
                .ForMember(dest => dest.ConvictedNotPardoned, opt => opt.MapFrom(s => s.csc_ConvictedNotPardoned))
                .ForMember(dest => dest.OutstandingCharges, opt => opt.MapFrom(s => s.csc_OutstandingCharges))

                .ForMember(dest => dest.HearAboutVolunteering, opt => opt.MapFrom(s => s.csc_HearAboutDetails))
                .ForMember(dest => dest.ReasonForVolunteering, opt => opt.MapFrom(s => s.csc_ReasonForVolunteering))
                .ForMember(dest => dest.AdditionalVolunteeringInfo, opt => opt.MapFrom(s => s.csc_AdditionalInformation))
                
                .ForMember(dest => dest.AgreementAcknowledged, opt => opt.MapFrom(s => s.csc_AgreeAcknowledge))

                .ForMember(dest => dest.DecisionStatus, opt => opt.MapFrom(s => s.csc_DecisionEnum))
                .ForMember(dest => dest.DecisionOn, opt => opt.MapFrom(s => s.csc_DecisionDate))               
                .ForMember(dest => dest.CancelledComment, opt => opt.MapFrom(s => s.csc_CancelledComment))
                .ForMember(dest => dest.CancelledOn, opt => opt.MapFrom(s => s.csc_CancelledDate))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(s => s.CreatedOn))
                ;
        }
    }
}