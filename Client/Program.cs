using Grpc.Core;
using Grpc.Net.Client;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using XXY.Cargo.GrpcClient;
using XXY.Cargo.GrpcLibrary;
using XXY.Common.Grpc;
using XXY.Common.Logger;

namespace Client
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = disco.TokenEndpoint,

            //    ClientId = "client",
            //    ClientSecret = "secret",
            //    Scope = "api1",

            //});

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client2",
                ClientSecret = "secret2",
                Scope = "api2 openid profile",
                UserName = "admin",
                Password = "123456",
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            #region get userinfo

            var userInfoClient = new HttpClient();
            userInfoClient.SetBearerToken(tokenResponse.AccessToken);
            
            var userInfoResponse = await userInfoClient.GetAsync(disco.UserInfoEndpoint);
            var userInfo = await userInfoResponse.Content.ReadAsStringAsync();

            Console.WriteLine(userInfo);
            #endregion

            #region call cargo grpc
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //var services = new ServiceCollection();
            //services.AddXXYLog4Net(x =>
            //{
            //    x.Log4netConfigPath = $"{AppContext.BaseDirectory}\\Configs\\log4net.config";
            //});
            //services.AddScoped<ClientLoggerInterceptor>();
            //services.AddCargoGrpcClients("http://localhost:40002");

            //var provider = services.BuildServiceProvider();
            //try
            //{
            //    var _fFCompanyClient = provider.GetService<FFCompany.FFCompanyClient>();
            //    var headers = new Metadata();
            //    headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");

            //    var ffrequest = new FFCompanyRequest();
            //    ffrequest.CustomerId = 234;
            //    var result = _fFCompanyClient.GetCustomerDetailInfo(ffrequest, headers);
            //}
            //catch (Exception ex)
            //{

            //}
            #endregion



            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:6001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.WriteLine("Hello World!");
            return;
        }
    }
}
