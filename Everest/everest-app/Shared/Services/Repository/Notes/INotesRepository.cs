using System;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.Notes
{
    public interface INotesRepository
    {
        public abstract Task<RepositoryResponseWrapper<List<Note>>> ListNotes();
        public abstract Task<RepositoryResponseWrapper<List<Note>>> SaveNoteAsync(Note note);
        public abstract Task<RepositoryResponseWrapper<List<Note>>> DeleteNoteAsync(Note note);
    }
}

