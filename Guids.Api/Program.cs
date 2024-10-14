using Microsoft.EntityFrameworkCore;
using Guids.Data.Models;
using Guids.Api.Managers;
using Guids.Api.Repository;
using Guids.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database"),
        builder => builder.MigrationsAssembly("Guids.Api")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

builder.Services.AddScoped<IGuidMetadataRepository, GuidMetadataRepository>();
builder.Services.AddScoped<IGuidManager, GuidManager>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapControllers();

app.Run();
