using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();  // Services definitions - this one is used to add the controllers
builder.Services.AddDbContext<DataContext>( opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.MapControllers(); // this is the middlerware - This one is used to map the endpoints so when a request is received this provide the endpoint

app.Run(); // This is the command that actually run the application - this unit Program.cs is the entry point to the application
