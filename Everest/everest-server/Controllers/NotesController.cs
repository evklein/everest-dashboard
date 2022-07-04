using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using everest_db_context_lib;
using everest_common.Models;

namespace everest_server.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly EverestDatabaseContext _dbContext;
    private static readonly string[] Summaries = new[]
{
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    public NotesController(EverestDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet(Name = "GetNotes")]
    public IEnumerable<Note> Get()
    {
        return _dbContext.Notes.ToList();
    }
}
