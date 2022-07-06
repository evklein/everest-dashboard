using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using everest_dashboard.Data;
using everest_dashboard.Shared.Services;
using everest_dashboard.Shared.Services.Http;
using everest_dashboard.Shared.Services.Http.Notes;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor server
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(@"http://localhost:5001/") });


builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<INotesService, NotesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
