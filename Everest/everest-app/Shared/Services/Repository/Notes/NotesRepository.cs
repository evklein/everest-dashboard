using System;
using everest_app.Data;
using everest_common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace everest_app.Shared.Services.Repository.Notes
{
    public class NotesRepository : INotesRepository
    {
        private readonly ApplicationDbContext _everestDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public NotesRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _everestDbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<RepositoryResponseWrapper<List<Note>>> ListNotes()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var notes = _everestDbContext.Notes.Include(n => n.Tags).Where(n => n.OwnerId.Equals(currentUser.Id)).ToList();
            notes.ForEach((note) =>
            {
                note.UpdatedTitle = note.Title;
                note.UpdatedContent = note.Content;
                note.Tags = note.Tags ?? new List<Tag>();
            });

            RepositoryResponseWrapper<List<Note>> responseWrapper = new()
            {
                Value = notes,
            };
            return responseWrapper;
        }

        public async Task<RepositoryResponseWrapper<List<Note>>> SaveNoteAsync(Note note)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            note.LastModified = DateTime.UtcNow;
            note.Title = note.UpdatedTitle;
            note.Content = note.UpdatedContent;
            note.OwnerId = currentUser.Id;

            foreach (var tag in note.Tags)
            {
                if (tag.DateCreated == DateTime.MinValue)
                {
                    tag.OwnerId = currentUser.Id;
                    tag.DateCreated = DateTime.UtcNow;

                    _everestDbContext.Tags.Add(tag);
                }
            }
            await _everestDbContext.SaveChangesAsync();

            if (string.IsNullOrEmpty(note.Title))
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error saving note: Note must have a title",
                    },
                };
            }

            try
            {
                var existingNote = await _everestDbContext.Notes.Where(n => n.Id == note.Id).SingleOrDefaultAsync();

                if (existingNote is not null)
                {
                    if (!existingNote.OwnerId.Equals(currentUser.Id))
                    {
                        return new RepositoryResponseWrapper<List<Note>>()
                        {
                            Success = false,
                            Error = new RepositoryResponseError()
                            {
                                ErrorMessage = "Error saving note: you are not the owner of this note",
                            },
                        };
                    }
                    existingNote.Title = note.UpdatedTitle;
                    existingNote.Content = note.UpdatedContent;
                    existingNote.Tags = note.Tags;
                }
                else
                {
                    await _everestDbContext.Notes.AddAsync(note);
                }
                await _everestDbContext.SaveChangesAsync();

                return await ListNotes();
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error saving note: Unexpected exception",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<List<Note>>> DeleteNoteAsync(Note note)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (!note.OwnerId.Equals(currentUser.Id))
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error deleting note: Note does not belong to your user",
                    },
                };
            }

            Note noteToDelete = await _everestDbContext.Notes.FindAsync(note.Id);

            if (noteToDelete is null)
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error deleting note: note could not be found",
                    },
                };
            }

            try
            {
                _everestDbContext.Notes.Remove(noteToDelete);
                await _everestDbContext.SaveChangesAsync();
                return await ListNotes();
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<Note>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error deleting note: Unexpected exception",
                        InnerException = ex,
                    },
                };
            }
        }
    }
}

