using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Hqs.Dto.Users;
using Hqs.IService.Logs;
using Hqs.IService.Users;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Hqs.AuthServer.AuthValidator
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public ResourceOwnerPasswordValidator(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
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
            else
            {
                await _logService.LogErrorAsync($"username:{context.UserName} or password invaild");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

        private List<Claim> UserClaims(UserDto user)
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
