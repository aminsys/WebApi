using Microsoft.EntityFrameworkCore;
using BokArkiv.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Eftersom vi inte har riktig databas använder vi oss av en in-memory databas
// för att kunna utföra CRUD operationer.
builder.Services.AddControllers();
builder.Services.AddDbContext<BookContext>(opt =>
opt.UseInMemoryDatabase("BooksDB"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
