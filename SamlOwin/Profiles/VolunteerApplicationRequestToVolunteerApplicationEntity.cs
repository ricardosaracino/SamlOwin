using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerApplicationRequestToVolunteerApplicationEntity: Profile
    {
        public VolunteerApplicationRequestToVolunteerApplicationEntity()
        {
            CreateMap<VolunteerApplicationRequest, csc_VolunteerApplication>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StatusCodeEnum, opt => opt.MapFrom(s => s.ApplicationStatus))
                .ForMember(dest => dest.csc_ActivityApplyingForEnum, opt => opt.MapFrom(s => s.ApplicationType))
                
                .ForMember(dest => dest.csc_AgreeAgeOfMajority, opt => opt.MapFrom(s => s.AgreeAgeOfMajority))
                .ForMember(dest => dest.csc_AgreeCanadianCitizen, opt => opt.MapFrom(s => s.AgreeCanadianCitizen))
                
                .ForMember(dest => dest.csc_OnInmateVisitingList, opt => opt.MapFrom(s => s.OnInmateVisitingList))
                .ForMember(dest => dest.csc_PersonallyKnowOffenders, opt => opt.MapFrom(s => s.PersonallyKnowOffenders))
                .ForMember(dest => dest.csc_ConvictedNotPardoned, opt => opt.MapFrom(s => s.ConvictedNotPardoned))
                .ForMember(dest => dest.csc_OutstandingCharges, opt => opt.MapFrom(s => s.OutstandingCharges))

                .ForMember(dest => dest.csc_HearAboutDetails, opt => opt.MapFrom(s => s.HearAboutVolunteering))
                .ForMember(dest => dest.csc_ReasonForVolunteering, opt => opt.MapFrom(s => s.ReasonForVolunteering))
                .ForMember(dest => dest.csc_AdditionalInformation, opt => opt.MapFrom(s => s.AdditionalVolunteeringInfo))
                
                .ForMember(dest => dest.csc_AgreeAcknowledge, opt => opt.MapFrom(s => s.AgreementAcknowledged))
                ;
        }
    }
}