using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEntityToVolunteerResponse : Profile
    {
        public VolunteerEntityToVolunteerResponse()
        {
            CreateMap<csc_Volunteer, VolunteerResponse>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerId))
                .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(s => s.csc_ReferenceNumber))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.csc_FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(s => s.csc_MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(s => s.csc_LastName))
                .ForMember(dest => dest.MaidenName, opt => opt.MapFrom(s => s.csc_MaidenName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => s.EmailAddress))
                .ForMember(dest => dest.SecondaryEmail, opt => opt.MapFrom(s => s.csc_SecondaryEmail))
                .ForMember(dest => dest.Citizenship, opt => opt.MapFrom(s => s.csc_Citizenship))
                .ForMember(dest => dest.PreferredLanguage,
                    opt => opt.MapFrom(s => s.csc_PreferredLanguageofCommunicationEnum))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(s => s.csc_GenderEnum))
                .ForMember(dest => dest.GenderOther, opt => opt.MapFrom(s => s.csc_GenderOther))
                .ForMember(dest => dest.HomeAddressCity, opt => opt.MapFrom(s => s.csc_HomeAddressCity))
                .ForMember(dest => dest.HomeAddressCountry, opt => opt.MapFrom(s => s.csc_HomeAddressCountry))
                .ForMember(dest => dest.HomeAddressLine1, opt => opt.MapFrom(s => s.csc_HomeAddressLine1))
                .ForMember(dest => dest.HomeAddressLine2, opt => opt.MapFrom(s => s.csc_HomeAddressLine2))
                .ForMember(dest => dest.HomeAddressPostalCode, opt => opt.MapFrom(s => s.csc_HomeAddressPostalCode))
                .ForMember(dest => dest.HomeAddressProvinceOrState,
                    opt => opt.MapFrom(s => s.csc_HomeAddressProvinceOrState))
                .ForMember(dest => dest.MailingAddressSameAsHomeAddress,
                    opt => opt.MapFrom(s => s.csc_MailingAddressSameAsHomeAddress))
                .ForMember(dest => dest.MailingAddressCity, opt => opt.MapFrom(s => s.csc_MailingAddressCity))
                .ForMember(dest => dest.MailingAddressCountry, opt => opt.MapFrom(s => s.csc_MailingAddressCountry))
                .ForMember(dest => dest.MailingAddressLine1, opt => opt.MapFrom(s => s.csc_MailingAddressLine1))
                .ForMember(dest => dest.MailingAddressLine2, opt => opt.MapFrom(s => s.csc_MailingAddressLine2))
                .ForMember(dest => dest.MailingAddressPostalCode,
                    opt => opt.MapFrom(s => s.csc_MailingAddressPostalCode))
                .ForMember(dest => dest.MailingAddressProvinceOrState,
                    opt => opt.MapFrom(s => s.csc_MailingAddressProvinceOrState))
                .ForMember(dest => dest.HomeTelephone, opt => opt.MapFrom(s => s.csc_HomeTelephone))
                .ForMember(dest => dest.BusinessTelephone, opt => opt.MapFrom(s => s.csc_BusinessTelephone))
                .ForMember(dest => dest.BusinessTelephoneExt, opt => opt.MapFrom(s => s.csc_BusinessTelephoneExt))
                .ForMember(dest => dest.OtherTelephone, opt => opt.MapFrom(s => s.csc_OtherTelephone))
                .ForMember(dest => dest.OtherTelephoneExt, opt => opt.MapFrom(s => s.csc_OtherTelephoneExt))
                .ForMember(dest => dest.AgreeContactedWork, opt => opt.MapFrom(s => s.csc_AgreeContactedWork))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(s => s.csc_Availability))
                ;
        }
    }
}