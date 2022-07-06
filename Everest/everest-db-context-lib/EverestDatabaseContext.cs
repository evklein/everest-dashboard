using Microsoft.EntityFrameworkCore;
using everest_common.Models;

namespace everest_db_context_lib;
public class EverestDatabaseContext : DbContext
{
    public DbSet<Note> Notes { get; set; }

    public string DbPath { get; }

    public EverestDatabaseContext()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join("", "everest.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    // See https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

}
