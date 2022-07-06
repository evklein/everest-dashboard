using System;
using everest_common.Models;

namespace everest_dashboard.Shared.Services.Http.Notes
{
    public interface INotesService
    {
        public abstract Task<IEnumerable<Note>> GetNotesAsync();
        public abstract Task<IEnumerable<Note>> SaveNoteAsync(Note note);
        public abstract Task<IEnumerable<Note>> DeleteNoteAsync(Note note);
    }
}

