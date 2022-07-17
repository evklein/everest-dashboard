using System;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.Tags
{
    public interface ITagRepository
    {
        public abstract Task<RepositoryResponseWrapper<Tag>> GetTag(string name);
        public abstract Task<RepositoryResponseWrapper<List<Tag>>> ListTagsForUser();
        public abstract Task<RepositoryResponseWrapper<List<Tag>>> ListTagsForUserWithExclusionsAndSearchValue(List<Tag> excludedTags, string searchValue);
    }
}

