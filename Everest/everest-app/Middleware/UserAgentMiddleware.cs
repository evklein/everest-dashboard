using System;
using everest_app.Shared.Services.Repository.UserAgents;
using everest_common.Models;

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
            string agentRequestEndpoint = context.Request.Path.Value?.Trim('/').ToUpper();
            switch (agentRequestEndpoint)
            {
                case "PINGUSERAGENT":
                    await Ping(context, userAgentRepository);
                    break;
                default:
                    await _next(context);
                    break;
            }
        }

        public async Task Ping(HttpContext context, IUserAgentRepository userAgentRepository)
        {
            if (context.Request.Method == "GET")
            {
                var queryArgs = context.Request.QueryString.Value?.Trim('?').Split('=');
                if (queryArgs[0] == "agent_id")
                {
                    Guid userAgentId = new Guid(queryArgs[1]);
                    var userAgentDirectives = await userAgentRepository.PingUserAgent(userAgentId);
                    if (userAgentDirectives.Success)
                    {
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsJsonAsync<List<UserAgentDirective>>(userAgentDirectives.Value ?? new List<UserAgentDirective>());
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Bad Request");
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Bad Request");
            }
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

