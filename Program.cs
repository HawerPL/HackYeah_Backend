using HackYeah_Backend.Data;
using HackYeah_Backend.Models.DTO;
using HackYeah_Backend.Models.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Bunkers.Any())
    {
        var geojson = File.ReadAllText("Data/Bunkers.geojson");
        var root = JsonDocument.Parse(geojson).RootElement;
        var features = JsonSerializer.Deserialize<List<GeoFeatureDto>>(root.GetProperty("features").GetRawText());

        var bunkers = features.Select(BunkerMapper.ToEntity).ToList();

        db.Bunkers.AddRange(bunkers);
        db.SaveChanges();
        Console.WriteLine($"Imported: {bunkers.Count} bunkers");
    }
}

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
