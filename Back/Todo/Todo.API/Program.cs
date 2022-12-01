using Todo.Persistence.Context;
using Todo.Application.Services;
using Microsoft.EntityFrameworkCore;
using Todo.Persistence.Repositories;
using Todo.Application.Services.Interfaces;
using Todo.Persistence.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepositoy>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

var database = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<TodoContext>(context => context.UseSqlite(database));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
