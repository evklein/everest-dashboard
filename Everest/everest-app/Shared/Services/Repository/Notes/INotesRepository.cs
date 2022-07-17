using System;
using everest_common.DataTransferObjects.Notes;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.Notes
{
    public interface INotesRepository
    {
        public abstract Task<RepositoryResponseWrapper<List<NoteListItem>>> ListNotes();
        public abstract Task<RepositoryResponseWrapper<Note>> GetNoteDetailsAsync(Guid noteId);
        public abstract Task<RepositoryResponseWrapper<SaveNoteResponseDataTransferObject>> SaveNoteAsync(Note note);
        public abstract Task<RepositoryResponseWrapper<List<NoteListItem>>> DeleteNoteAsync(Guid noteId);
    }
}

