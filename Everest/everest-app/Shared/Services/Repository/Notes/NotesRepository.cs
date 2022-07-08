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

        public RepositoryResponseWrapper<List<Note>> ListNotes()
        {
            var notes = _everestDbContext.Notes.ToList();
            notes.ForEach((note) =>
            {
                note.UpdatedTitle = note.Title;
                note.UpdatedContent = note.Content;
            });

            RepositoryResponseWrapper<List<Note>> responseWrapper = new()
            {
                Value = notes,
            };
            return responseWrapper;
        }

        public async Task<RepositoryResponseWrapper<List<Note>>> SaveNoteAsync(Note note)
        {
            note.LastModified = DateTime.UtcNow;
            note.Title = note.UpdatedTitle;
            note.Content = note.UpdatedContent;

            if (string.IsNullOrEmpty(note.Title))
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError("Error saving note: Note must have a title"),
                };
            }

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

                return ListNotes();
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError("Error saving note: Unexpected exception")
                    {
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<List<Note>>> DeleteNoteAsync(Note note)
        {
            Note noteToDelete = await _everestDbContext.Notes.FindAsync(note.Id);

            if (noteToDelete is null)
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError("Error deleting note: note could not be found"),
                };
            }

            try
            {
                _everestDbContext.Notes.Remove(noteToDelete);
                await _everestDbContext.SaveChangesAsync();
                return ListNotes();
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError("Error deleting note: Unexpected exception")
                    {
                        InnerException = ex,
                    },
                };
            }
        }
    }
}

