using System;
using everest_app.Data;
using everest_app.Shared.Services.Queries.Notes;
using everest_common.DataTransferObjects.Notes;
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
        private readonly NoteQueries _noteQueries;

        public NotesRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _everestDbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

            _noteQueries = new NoteQueries(); // TODO: Consider making this an injectable service.
        }

        public async Task<RepositoryResponseWrapper<List<NoteListItem>>> ListNotes()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var noteListItems = await _noteQueries.GetNoteListItemsAsync(_everestDbContext, currentUser.Id);

                 return new RepositoryResponseWrapper<List<NoteListItem>>
                 {
                    Value = noteListItems,
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<NoteListItem>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error getting list of notes: Unknown exception",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<Note>> GetNoteDetailsAsync(Guid noteId)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var note = await _everestDbContext.Notes.Include(n => n.Tags)
                                                        .Where(n => n.OwnerId == currentUser.Id)
                                                        .Where(n => n.Id == noteId)
                                                        .SingleOrDefaultAsync();
                if (note is not null)
                {
                    note.UpdatedTitle = note.Title;
                    note.UpdatedContent = note.Content;

                    return new RepositoryResponseWrapper<Note>()
                    {
                        Value = note,
                    };
                }
                else
                {
                    return new RepositoryResponseWrapper<Note>()
                    {
                        Success = false,
                        Error = new RepositoryResponseError()
                        {
                            ErrorMessage = "Error getting note: Note could not be found.",
                        },
                    };
                }
                                                        
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<Note>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error getting note: Unknown exception.",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<SaveNoteResponseDataTransferObject>> SaveNoteAsync(Note note)
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
                return new RepositoryResponseWrapper<SaveNoteResponseDataTransferObject>()
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
                        return new RepositoryResponseWrapper<SaveNoteResponseDataTransferObject>()
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

                return new RepositoryResponseWrapper<SaveNoteResponseDataTransferObject>
                {
                    Value = new SaveNoteResponseDataTransferObject()
                    {
                        SavedNote = note,
                        NoteListItems = await _noteQueries.GetNoteListItemsAsync(_everestDbContext, currentUser.Id),
                    },
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<SaveNoteResponseDataTransferObject>()
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

        public async Task<RepositoryResponseWrapper<List<NoteListItem>>> DeleteNoteAsync(Guid noteId)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var noteToDelete = await _everestDbContext.Notes.FindAsync(noteId);

                if (noteToDelete is not null)
                {
                    if (!noteToDelete.OwnerId.Equals(currentUser.Id))
                    {
                        return new RepositoryResponseWrapper<List<NoteListItem>>()
                        {
                            Success = false,
                            Error = new RepositoryResponseError()
                            {
                                ErrorMessage = "Error deleting note: Note does not belong to your user",
                            },
                        };
                    }

                    _everestDbContext.Notes.Remove(noteToDelete);
                    await _everestDbContext.SaveChangesAsync();
                    return await ListNotes();
                }
                else
                {
                    return new RepositoryResponseWrapper<List<NoteListItem>>()
                    {
                        Success = false,
                        Error = new RepositoryResponseError()
                        {
                            ErrorMessage = "Error deleting note: note could not be found",
                        },
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<NoteListItem>>()
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

