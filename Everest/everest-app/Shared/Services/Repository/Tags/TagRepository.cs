using System;
using everest_app.Data;
using everest_common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace everest_app.Shared.Services.Repository.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _everestDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public TagRepository(ApplicationDbContext everestDbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _everestDbContext = everestDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<RepositoryResponseWrapper<Tag>> GetTag(string name)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var tag = _everestDbContext.Tags.Where(t => t.OwnerId == currentUser.Id)
                                                .Where(t => t.Name.Equals(name))
                                                .FirstOrDefault();
                return new RepositoryResponseWrapper<Tag>()
                {
                    Value = tag,
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<Tag>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error getting Tag with name: unknown exception",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<List<Tag>>> ListTagsForUser()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var tagsForUsers = _everestDbContext.Tags.Where(t => t.OwnerId == currentUser.Id).ToList();
                return new RepositoryResponseWrapper<List<Tag>>()
                {
                    Value = tagsForUsers,
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<Tag>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error getting Tags: unknown exception",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<List<Tag>>> ListTagsForUserWithExclusionsAndSearchValue(List<Tag> excludedTags, string searchValue = null)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var tagsForUsersWithExclusions = _everestDbContext.Tags.Where(t => t.OwnerId == currentUser.Id)
                                                                       .Where(t => !excludedTags.Contains(t))
                                                                       .Where(t => string.IsNullOrEmpty(searchValue) ?
                                                                           true :
                                                                           EF.Functions.Like(t.Name, $"%{searchValue}%"))
                                                                       .OrderBy(t => t.Name)
                                                                       .Take(25)
                                                                       .ToList();
                return new RepositoryResponseWrapper<List<Tag>>()
                {
                    Value = tagsForUsersWithExclusions,
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<Tag>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error getting Tags with exclusions: unknown exception",
                        InnerException = ex,
                    },
                };
            }
        }
    }
}

