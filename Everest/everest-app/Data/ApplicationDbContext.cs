using everest_common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace everest_app.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

