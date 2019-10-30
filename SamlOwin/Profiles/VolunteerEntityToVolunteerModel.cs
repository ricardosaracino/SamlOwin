using AutoMapper;
using CrmEarlyBound;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerEntityToVolunteerModel : Profile
    {
        public VolunteerEntityToVolunteerModel()
        {
            CreateMap<csc_Volunteer, Volunteer>()
                // explicitly copy id, joins Id was empty
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.csc_VolunteerId))
                .ForMember(dest => dest.CanApplyCac, opt => opt.MapFrom(s => s.csc_CanApplyCAC))
                .ForMember(dest => dest.CanApplyCsc, opt => opt.MapFrom(s => s.csc_CanApplyGeneral))
                .ForMember(dest => dest.CanApplyReac, opt => opt.MapFrom(s => s.csc_CanApplyREAC))
                .ForMember(dest => dest.EmailVerifiedOn, opt => opt.MapFrom(s => s.csc_EmailVerifiedOn))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.csc_FirstName))
                .ForMember(dest => dest.HomeAddressCity, opt => opt.MapFrom(s => s.csc_HomeAddressCity))
                .ForMember(dest => dest.HomeAddressCountry, opt => opt.MapFrom(s => s.csc_HomeAddressCountry))
                .ForMember(dest => dest.HomeAddressLine1, opt => opt.MapFrom(s => s.csc_HomeAddressLine1))
                .ForMember(dest => dest.HomeAddressLine2, opt => opt.MapFrom(s => s.csc_HomeAddressLine2))
                .ForMember(dest => dest.HomeAddressPostalCode, opt => opt.MapFrom(s => s.csc_HomeAddressPostalCode))
                .ForMember(dest => dest.HomeAddressProvinceOrState, opt => opt.MapFrom(s => s.csc_HomeAddressProvinceOrState))
                .ForMember(dest => dest.HomeTelephone, opt => opt.MapFrom(s => s.csc_HomeTelephone))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(s => s.csc_LastName))
                .ForMember(dest => dest.MailingAddressCity, opt => opt.MapFrom(s => s.csc_MailingAddressCity))
                .ForMember(dest => dest.MailingAddressCountry, opt => opt.MapFrom(s => s.csc_MailingAddressCountry))
                .ForMember(dest => dest.MailingAddressLine1, opt => opt.MapFrom(s => s.csc_MailingAddressLine1))
                .ForMember(dest => dest.MailingAddressLine2, opt => opt.MapFrom(s => s.csc_MailingAddressLine2))
                .ForMember(dest => dest.MailingAddressPostalCode, opt => opt.MapFrom(s => s.csc_MailingAddressPostalCode))
                .ForMember(dest => dest.MailingAddressProvinceOrState, opt => opt.MapFrom(s => s.csc_MailingAddressProvinceOrState))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(s => s.csc_MiddleName))
                .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(s => s.csc_ReferenceNumber))
                ;
        }
    }
}