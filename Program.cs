using Microsoft.EntityFrameworkCore;
using MyApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); 

builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(
        "Server=DESKTOP-76A1KAF\\SQLEXPRESS;Database=test;Trusted_Connection=True;TrustServerCertificate=True"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); 

var summaries = new[]
{
    "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Sweltering","Scorching"
};

app.MapGet("/movies", (TestContext db) =>
{
    return db.MovieMovies.ToList();
});

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1,5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20,55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date,int TemperatureC,string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}