using System;
using everest_app.Shared.Services.Repository.UserAgents;

namespace everest_app.Middleware
{
    public class UserAgentMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAgentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserAgentRepository userAgentRepository)
        {
            if (context.Request.Path.Value.ToUpper().Equals("/PINGUSERAGENT"))
            {
                await Ping(context);
                return;
            }

            await _next(context);
        }

        public async Task Ping(HttpContext httpContext)
        {
            var body = httpContext.Request.Body;
            httpContext.Response.StatusCode = 401; //UnAuthorized
            await httpContext.Response.WriteAsync("Invalid User Key");
            return;

        }

        public string GetCurrentDirectives()
        {
            return string.Empty;
        }

        public string SaveCurrentUserAgentSettings()
        {
            return string.Empty;
        }
    }

    public static class UserAgentMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserAgentMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserAgentMiddleware>();
        }
    }
}

