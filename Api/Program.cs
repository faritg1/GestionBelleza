using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<GestionBellezaDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("MySqlContext");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

