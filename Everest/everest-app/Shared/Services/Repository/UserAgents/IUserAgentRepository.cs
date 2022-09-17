using System;
using everest_common.Enumerations;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.UserAgents
{
    public interface IUserAgentRepository
    {
        public abstract RepositoryResponseWrapper<List<UserAgent>> GetUserAgents();
        public abstract Task<RepositoryResponseWrapper<List<UserAgent>>> AddUserAgent(UserAgent userAgent);
        public abstract RepositoryResponseWrapper<List<UserAgent>> FindCurrentlyConnectedUserAgents(UserAgentStaleness staleness = UserAgentStaleness.FiveMinutes);
        public abstract Task<RepositoryResponseWrapper<List<UserAgentDirective>>> GetCurrentDirectivesForUserAgent(Guid userAgentId);
        public abstract Task<RepositoryResponseWrapper<List<UserAgentDirective>>> PingUserAgent(Guid userAgentId);
    }
}

