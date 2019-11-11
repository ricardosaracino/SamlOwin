using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class VolunteerSelfIdentificationRequestToVolunteerEntity : Profile
    {
        public VolunteerSelfIdentificationRequestToVolunteerEntity()
        {
            CreateMap<VolunteerSelfIdentificationRequest, csc_Volunteer>()
                // Ignore Id (readonly), retch the model with the context and update it with mapper
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.csc_AboriginalPerson,
                    opt => opt.MapFrom(s => s.AboriginalTypes == null
                        ? null
                        : new OptionSetValueCollection(
                            s.AboriginalTypes.ToList().ConvertAll(v => new OptionSetValue(v)))))
                .ForMember(dest => dest.csc_CultureGroups,
                    opt => opt.MapFrom(s => s.CultureTypes == null
                        ? null
                        : new OptionSetValueCollection(s.CultureTypes.ToList().ConvertAll(v => new OptionSetValue(v)))))
                .ForMember(dest => dest.csc_Disabilities,
                    opt => opt.MapFrom(s => s.DisabilityTypes == null
                        ? null
                        : new OptionSetValueCollection(
                            s.DisabilityTypes.ToList().ConvertAll(v => new OptionSetValue(v)))))
                .ForMember(dest => dest.csc_MinorityGroups,
                    opt => opt.MapFrom(s => s.MinorityTypes == null
                        ? null
                        : new OptionSetValueCollection(s.MinorityTypes.ToList()
                            .ConvertAll(v => new OptionSetValue(v)))))
                .ForMember(dest => dest.csc_PermissiontouseSelfIdentification,
                    opt => opt.MapFrom(s => s.AgreeSelfIdentification))
                ;
        }
    }
}