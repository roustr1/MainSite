using System;
using System.Threading.Tasks;
using Application.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace MainSite.Middleware
{
    public class UserAreCreateMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAreCreateMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, IUsersService userService)
        {
            var user = userService.GetUserBySystemName(context.User);
            user.LastIpAddress = GetIP(context);
            user.LastActivityDate = DateTime.Now;
            userService.UpdateUser(user);


            await _next.Invoke(context);
        }


        private String GetIP(HttpContext context)
        {
            var ip =
                context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.MapToIPv4().ToString();


            return ip;
        }
    }
}
