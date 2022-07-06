using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using everest_db_context_lib;
using everest_common.Models;

namespace everest_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly EverestDatabaseContext _dbContext;


    public NotesController(EverestDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<Note> ListNotes()
    {
        var notes = _dbContext.Notes.ToList();
        notes.ForEach((note) =>
        {
            note.UpdatedTitle = note.Title;
            note.UpdatedContent = note.Content;
        });
        return notes;
    }

    [HttpPost]
    public async Task<IEnumerable<Note>> SaveNote(Note note)
    {
        note.LastModified = DateTime.UtcNow;
        note.Title = note.UpdatedTitle;
        note.Content = note.UpdatedContent;

        try
        {
            var existingNote = await _dbContext.Notes.FindAsync(note.Id);

            if (existingNote is not null)
            {
                existingNote.Title = note.UpdatedTitle;
                existingNote.Content = note.UpdatedContent;
            }
            else
            {
                await _dbContext.Notes.AddAsync(note);
            }

            await _dbContext.SaveChangesAsync();

            var updatedNotesList = _dbContext.Notes.ToList();
            updatedNotesList.ForEach((note) =>
            {
                note.UpdatedTitle = note.Title;
                note.UpdatedContent = note.Content;
            });

            return updatedNotesList;
        }
        catch(Exception ex)
        {
            var x = ex;
        }

        return null;
    }

    [HttpDelete("{id}")]
    public async Task<IEnumerable<Note>> DeleteNote(Guid id)
    {
        Note noteToDelete = await _dbContext.Notes.FindAsync(id);

        if (noteToDelete is not null)
        {
            _dbContext.Notes.Remove(noteToDelete);
            await _dbContext.SaveChangesAsync();

            var updatedNotesList = _dbContext.Notes.ToList();
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
