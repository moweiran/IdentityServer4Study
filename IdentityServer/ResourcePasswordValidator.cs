using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ResourcePasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "admin" && context.Password == "123456")
            {
                context.Result = new GrantValidationResult(
                    subject: "123",
                    authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                    claims: GetUserClaims());
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            return Task.FromResult(0);
        }

        //可以根据需要设置相应的Claim/需要实现IProfileService接口
        private Claim[] GetUserClaims()
        {
            return new Claim[]
            {
                new Claim("userId","110"),
                new Claim(JwtClaimTypes.Name,"林辉"),
                new Claim(JwtClaimTypes.Role,"菜鸡"),
                new Claim(JwtClaimTypes.NickName,"Sarco"),
                new Claim(JwtClaimTypes.GivenName,"SarcoTest"),
                new Claim(JwtClaimTypes.PhoneNumber,"186221085730"),
            };
        }
    }
}
