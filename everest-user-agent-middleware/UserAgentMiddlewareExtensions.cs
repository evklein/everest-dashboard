using Microsoft.AspNetCore.Builder;

namespace everest_user_agent_middleware;
public static class UserAgentMiddlewareExtensions
{
    public static IApplicationBuilder UseUserAgentMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserAgentMiddleware>();
    }
}