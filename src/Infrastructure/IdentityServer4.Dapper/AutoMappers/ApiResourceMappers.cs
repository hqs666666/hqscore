using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Dapper.AutoMappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>()).CreateMapper();
        }

        internal static IMapper Mapper;

        public static ApiResource ToModel(this Model.ApiResource entity)
        {
            return entity == null ? null : Mapper.Map<ApiResource>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Model.ApiResource ToEntity(this ApiResource model)
        {
            return model == null ? null : Mapper.Map<Model.ApiResource>(model);
        }
    }

    public class ApiResourceMapperProfile : Profile
    {
        public ApiResourceMapperProfile()
        {
            CreateMap<Model.ApiResource, ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                .ReverseMap();

            CreateMap<Model.ApiResourceClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<Model.ApiSecret, Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<Model.ApiScope, Scope>(MemberList.Destination)
                .ConstructUsing(src => new Scope())
                .ReverseMap();

            CreateMap<Model.ApiScopeClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}
