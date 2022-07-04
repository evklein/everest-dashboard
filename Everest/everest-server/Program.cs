using Microsoft.EntityFrameworkCore;
using everest_db_context_lib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EverestDatabaseContext>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();

