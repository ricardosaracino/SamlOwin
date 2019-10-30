using AutoMapper;
using Microsoft.Xrm.Sdk;
using SamlOwin.Models;

namespace SamlOwin.Profiles
{
    public class EntityReferenceToObjectResponse: Profile
    {
        public EntityReferenceToObjectResponse()
        {
            CreateMap<EntityReference, ObjectResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.Name))
                ;
        }
    }
}