using System;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.Notes
{
    public interface INotesRepository
    {
        public abstract IEnumerable<Note> ListNotes();
        public abstract Task<IEnumerable<Note>> SaveNoteAsync(Note note);
        public abstract Task<IEnumerable<Note>> DeleteNoteAsync(Note note);
    }
}

