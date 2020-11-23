using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controller
{
    [Route("identity")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        public IActionResult Get()
        {
            //var userInfoClient = new HttpClient();
            //userInfoClient.SetBearerToken(HttpContext.User.);

            //var userInfoResponse = await userInfoClient.GetAsync(disco.UserInfoEndpoint);
            //var userInfo = userInfoResponse.Content.ReadAsStringAsync();
            string authHeader = this.Request.Headers["Authorization"];//Header中的token
            string token = authHeader.Substring("Bearer ".Length).Trim();
            //var serializer = new JsonNetSerializer();

            var user = HttpContext.User;
            var claim = User.Claims.Where(a => a.Type == JwtClaimTypes.Subject).FirstOrDefault();
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
