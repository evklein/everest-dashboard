using System;
using everest_app.Data;
using everest_common.Models;
using Microsoft.AspNetCore.Identity;

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
    }
}

