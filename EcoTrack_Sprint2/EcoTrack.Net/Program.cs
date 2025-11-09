using EcoTrack.Application.Services;
using EcoTrack.Domain.Repositories;
using EcoTrack.Infrastructure.Data;
using EcoTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use InMemory DB for demonstration (Sprint 2 deliverable). In production switch to Oracle via connection string.
builder.Services.AddDbContext<EcoTrackDbContext>(opt => opt.UseInMemoryDatabase("EcoTrackDb"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

app.Run();
