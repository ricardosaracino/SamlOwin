using AutoMapper;
using Microsoft.Xrm.Sdk;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class EntityReferenceToReferenceResponse : Profile
    {
        public EntityReferenceToReferenceResponse()
        {
            CreateMap<EntityReference, ReferenceResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.Name))
                ;
        }
    }
}