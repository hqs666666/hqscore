using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Hqs.IService.Users;
using Hqs.Model.Users;
using IdentityModel;
using IdentityServer4.Validation;

namespace Hqs.AuthServer.AuthValidator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public readonly IUserService _userService;

        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            _userService = userService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _userService.GetUser(context.UserName, context.Password);
            if (user != null)
            {
                context.Result = new GrantValidationResult(
                    user.Id,
                    OidcConstants.AuthenticationMethods.Password,
                    DateTime.UtcNow,
                    UserClaims(user));
            }
            return Task.CompletedTask;
        }

        private List<Claim> UserClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim("nick_name", user.NickName ?? ""),
                new Claim("email", user.Email ?? ""),
                new Claim("name", user.Name ?? ""),
                new Claim("sub", user.Id ?? "")
            };
        }
    }
}
