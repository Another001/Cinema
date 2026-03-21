using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Services;
using MyApi.Repositories;
using MyApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); 
// 2. Đăng ký Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// 3. Đăng ký Service 
// (Vì IUserService kế thừa IServiceScoped nên hệ thống DI sẽ hiểu)
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMovieService, MockMovieService>();

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

app.Run();
