using AutoMapper;
using Hqs.Dto.Users;
using Hqs.Model.Users;

namespace Hqs.Service
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            #region Users

            CreateMap<UserDto, UserProfile>();
            CreateMap<UserProfile, UserDto>();

            #endregion
        }
    }
}
