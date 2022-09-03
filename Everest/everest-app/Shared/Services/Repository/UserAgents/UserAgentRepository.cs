﻿using System;
using everest_app.Data;
using everest_app.Shared.Services.Repository.Tags;
using everest_common.Enumerations;
using everest_common.Models;
using Microsoft.AspNetCore.Identity;

namespace everest_app.Shared.Services.Repository.UserAgents
{
    public class UserAgentRepository : IUserAgentRepository
    {
        private readonly ApplicationDbContext _everestDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITagRepository _tagRepository;

        public UserAgentRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, ITagRepository tagRepository)
        {
            _everestDbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tagRepository = tagRepository;
        }

        public RepositoryResponseWrapper<List<UserAgent>> GetUserAgents()
        {
            var userAgents = _everestDbContext.UserAgents.ToList();

            return new RepositoryResponseWrapper<List<UserAgent>>()
            {
                Value = userAgents,
            };
        }

        public async Task<RepositoryResponseWrapper<List<UserAgent>>> AddUserAgent(UserAgent userAgent)
        {
            var existingUserAgent = await _everestDbContext.UserAgents.FindAsync(userAgent.Id);

            if (existingUserAgent is not null)
            {
                return new RepositoryResponseWrapper<List<UserAgent>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = $"Error adding UserAgent: UserAgent with ID {userAgent.Id} already found.",
                    },
                };
            }

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            userAgent.OwnerId = currentUser.Id;

            await _everestDbContext.UserAgents.AddAsync(userAgent);
            await _everestDbContext.SaveChangesAsync();

            return GetUserAgents();
        }

        public RepositoryResponseWrapper<List<UserAgent>> FindCurrentlyConnectedUserAgents(UserAgentStaleness staleness = UserAgentStaleness.FiveMinutes)
        {
            var numberOfMinutes = staleness switch
            {
                UserAgentStaleness.OneMinute => 1,
                UserAgentStaleness.FiveMinutes => 5,
                UserAgentStaleness.ThirtyMinutes => 30,
                UserAgentStaleness.OneHour => 60,
                _ => 1,
            };

            var timeout = new TimeSpan(0, numberOfMinutes, 0);
            var userAgentsWithRecentPings = _everestDbContext.UserAgents
                                                             .ToList() 
                                                             .Where(ua => DateTime.UtcNow - ua.LastPing < timeout)
                                                             .ToList();

            return new RepositoryResponseWrapper<List<UserAgent>>()
            {
                Value = userAgentsWithRecentPings,
            };
        }
    }
}

