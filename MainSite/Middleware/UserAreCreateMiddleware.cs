using System.Threading.Tasks;
using Application.Services.Users;
using Microsoft.AspNetCore.Http;

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
            
                userService.GetUserBySystemName(context.User);       
               


            await _next.Invoke(context);
        }
    }
}
