using System;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.UserAgents
{
    public interface IUserAgentRepository
    {
        public abstract RepositoryResponseWrapper<List<UserAgent>> GetUserAgents();
        public abstract Task<RepositoryResponseWrapper<List<UserAgent>>> AddUserAgent(UserAgent userAgent);
    }
}

