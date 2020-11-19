using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
            {
                // web applicatiion 1 API specific scopes
                new ApiScope("api1", "My API"),
                //new ApiScope("api1.add","My API add"),

                // web application 2 API specific scopes
                new ApiScope("api2","My API2"),
                //new ApiScope("api2.add","My API2 add")
            };

        public static IEnumerable<Client> Clients => new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "client2",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // 账户密码验证方式

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret2".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                        IdentityServerConstants.StandardScopes.Profile
                    },

                }
            };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("Api1", "Api1")
            {
                Scopes = { "api1" }
            },
            new ApiResource("Api2","Api2")
            {
                Scopes = {"api2"}
            }
        };

        /// <summary>
        /// 针对用户密码验证方式需要比对的账户
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers => new List<TestUser>
        {
           new TestUser()
           {
                SubjectId="113123",
                Username="YCZ",
                Password="123456",
                ProviderName="",        //获取或设置提供程序名称。
                ProviderSubjectId="",   //获取或设置提供程序主题标识符。
                IsActive=true,          //获取或设置用户是否处于活动状态。
                Claims=new List<Claim>() //身份  这是一个List 里面放的是包含的身份。 身份的概念就类似于(一个人是教师/父亲这种标识性的东西可以支持多个)
                {
                    new Claim("userId","110"),
                    new Claim(JwtClaimTypes.Name,""),
                    new Claim(JwtClaimTypes.Role,"菜鸡"),
                    new Claim(JwtClaimTypes.NickName,"Sarco"),
                    new Claim(JwtClaimTypes.GivenName,"SarcoTest"),
                    new Claim(JwtClaimTypes.PhoneNumber,"186221085730"),
                }
            }
        };
    }
}
