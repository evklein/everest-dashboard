using System;
using everest_app.Data;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.Notes
{
    public class NotesRepository : INotesRepository
    {
        private readonly ApplicationDbContext _everestDbContext;

        public NotesRepository(ApplicationDbContext dbContext)
        {
            _everestDbContext = dbContext;
        }

        public IEnumerable<Note> ListNotes()
        {
            var notes = _everestDbContext.Notes.ToList();
            notes.ForEach((note) =>
            {
                note.UpdatedTitle = note.Title;
                note.UpdatedContent = note.Content;
            });
            return notes;
        }

        public async Task<IEnumerable<Note>> SaveNoteAsync(Note note)
        {
            note.LastModified = DateTime.UtcNow;
            note.Title = note.UpdatedTitle;
            note.Content = note.UpdatedContent;

            try
            {
                var existingNote = await _everestDbContext.Notes.FindAsync(note.Id);

                if (existingNote is not null)
                {
                    existingNote.Title = note.UpdatedTitle;
                    existingNote.Content = note.UpdatedContent;
                }
                else
                {
                    await _everestDbContext.Notes.AddAsync(note);
                }

                await _everestDbContext.SaveChangesAsync();

                var updatedNotesList = _everestDbContext.Notes.ToList();
                updatedNotesList.ForEach((note) =>
                {
                    note.UpdatedTitle = note.Title;
                    note.UpdatedContent = note.Content;
                });

                return updatedNotesList;
            }
            catch (Exception ex)
            {
                var x = ex;
            }

            return null;
        }

        public async Task<IEnumerable<Note>> DeleteNoteAsync(Note note)
        {
            Note noteToDelete = await _everestDbContext.Notes.FindAsync(note.Id);

            if (noteToDelete is not null)
            {
                _everestDbContext.Notes.Remove(noteToDelete);
                await _everestDbContext.SaveChangesAsync();

                var updatedNotesList = _everestDbContext.Notes.ToList();
                updatedNotesList.ForEach((note) =>
                {
                    note.UpdatedTitle = note.Title;
                    note.UpdatedContent = note.Content;
                });

                return updatedNotesList;
            }

            return null; // todo
        }
    }
}

