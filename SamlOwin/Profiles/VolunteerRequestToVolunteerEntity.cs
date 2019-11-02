using AutoMapper;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerRequestToVolunteerEntity : Profile
    {
        public VolunteerRequestToVolunteerEntity()
        {
            CreateMap<VolunteerRequest, csc_Volunteer>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.csc_FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(dest => dest.csc_HomeAddressCity, opt => opt.MapFrom(s => s.HomeAddressCity))
                .ForMember(dest => dest.csc_HomeAddressCountry,
                    opt => opt.MapFrom(s =>
                        new EntityReference(csc_Country.EntityLogicalName, s.HomeAddressCountry.Id)))
                .ForMember(dest => dest.csc_HomeAddressLine1, opt => opt.MapFrom(s => s.HomeAddressLine1))
                .ForMember(dest => dest.csc_HomeAddressLine2, opt => opt.MapFrom(s => s.HomeAddressLine2))
                .ForMember(dest => dest.csc_HomeAddressPostalCode, opt => opt.MapFrom(s => s.HomeAddressPostalCode))
                .ForMember(dest => dest.csc_HomeAddressProvinceOrState,
                    opt => opt.MapFrom(s =>
                        new EntityReference(csc_ProvinceOrState.EntityLogicalName, s.HomeAddressProvinceOrState.Id)))
                .ForMember(dest => dest.csc_HomeTelephone, opt => opt.MapFrom(s => s.HomeTelephone))
                .ForMember(dest => dest.csc_LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(dest => dest.csc_MailingAddressCity, opt => opt.MapFrom(s => s.MailingAddressCity))
                .ForMember(dest => dest.csc_MailingAddressCountry, opt => opt.MapFrom(s =>
                    new EntityReference(csc_Country.EntityLogicalName, s.MailingAddressCountry.Id)))
                .ForMember(dest => dest.csc_MailingAddressLine1, opt => opt.MapFrom(s => s.MailingAddressLine1))
                .ForMember(dest => dest.csc_MailingAddressLine2, opt => opt.MapFrom(s => s.MailingAddressLine2))
                .ForMember(dest => dest.csc_MailingAddressPostalCode,
                    opt => opt.MapFrom(s => s.MailingAddressPostalCode))
                .ForMember(dest => dest.csc_MailingAddressProvinceOrState,
                    opt => opt.MapFrom(s =>
                        new EntityReference(csc_ProvinceOrState.EntityLogicalName, s.MailingAddressProvinceOrState.Id)))
                .ForMember(dest => dest.csc_MiddleName, opt => opt.MapFrom(s => s.MiddleName))
                ;
        }
    }
}