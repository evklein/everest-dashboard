using System;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.Tags
{
    public interface ITagRepository
    {
        public abstract Task<RepositoryResponseWrapper<List<Tag>>> ListTagsForUser();
    }
}

