using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Dapper.AutoMappers
{
    public static class PersistedGrantMapper
    {
        static PersistedGrantMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>()).CreateMapper();
        }

        internal static IMapper Mapper { get; set; }

        public static PersistedGrant ToModel(this Model.PersistedGrant entity)
        {
            return entity == null ? null : Mapper.Map<PersistedGrant>(entity);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Model.PersistedGrant ToEntity(this PersistedGrant model)
        {
            return model == null ? null : Mapper.Map<Model.PersistedGrant>(model);
        }

        /// <summary>
        /// Updates an entity from a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        public static void UpdateEntity(this PersistedGrant model, Model.PersistedGrant entity)
        {
            Mapper.Map(model, entity);
        }
    }

    public class PersistedGrantMapperProfile : Profile
    {
        public PersistedGrantMapperProfile()
        {
            CreateMap<Model.PersistedGrant, PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
