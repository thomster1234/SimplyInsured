
using Microsoft.EntityFrameworkCore;
using VerzekeringApi.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=verzekeringen.db"));

builder.Services.AddControllers();          // Controllers i.p.v. minimal endpoints
builder.Services.AddEndpointsApiExplorer(); // Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // Activeer attribute-routed controllers

app.Run();
