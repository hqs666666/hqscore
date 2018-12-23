using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Dapper.AutoMappers
{
    public static class IdentityResourceMapper
    {
        static IdentityResourceMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>()).CreateMapper();
        }

        internal static IMapper Mapper { get; set; }

        public static IdentityResource ToModel(this Model.IdentityResource entity)
        {
            return entity == null ? null : Mapper.Map<IdentityResource>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Model.IdentityResource ToEntity(this IdentityResource model)
        {
            return model == null ? null : Mapper.Map<Model.IdentityResource>(model);
        }
    }

    public class IdentityResourceMapperProfile : Profile
    {
        public IdentityResourceMapperProfile()
        {
            CreateMap<Model.IdentityResource, IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityResource())
                .ReverseMap();

            CreateMap<Model.IdentityClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}
