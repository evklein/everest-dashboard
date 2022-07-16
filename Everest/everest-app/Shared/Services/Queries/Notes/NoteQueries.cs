using System;
using everest_app.Data;
using everest_common.DataTransferObjects.Notes;
using everest_common.Models;
using Microsoft.EntityFrameworkCore;

namespace everest_app.Shared.Services.Queries.Notes
{
    public class NoteQueries
    {
        public async Task<List<NoteListItem>> GetNoteListItemsAsync(ApplicationDbContext applicationDbContext, string identityOwnerId, string titleSearchValue = null, int count = 100)
        {
            return await applicationDbContext.Notes.Include(n => n.Tags)
                                                   .Where(n => n.OwnerId.Equals(identityOwnerId))
                                                   .Where(n => string.IsNullOrEmpty(titleSearchValue) ? true : n.Title.Contains(titleSearchValue, StringComparison.InvariantCultureIgnoreCase))
                                                   .Select(n => new NoteListItem
                                                   {
                                                       NoteId = n.Id,
                                                       NoteTitle = n.Title,
                                                       NoteDateModified = n.LastModified,
                                                       NoteTags = n.Tags.ToList() ?? new List<Tag>(),
                                                   })
                                                   .OrderBy(nli => nli.NoteDateModified)
                                                   .Take(count)
                                                   .ToListAsync();
        }
    }
}

