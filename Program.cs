using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Services;
using MyApi.Repositories;
using MyApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true; 
  });
builder.Services.AddCors(options => {
  // SỬA-DEPLOY: đọc danh sách domain FE từ env FRONTEND_URL (cách nhau bằng ';').
  // Để làm gì: local là http://localhost:3000, deploy là https://xxx.trycloudflare.com / https://xxx.vercel.app.
  // Nếu không đọc env mà hardcode thì lên public URL sẽ bị chặn CORS (API + SignalR lỗi).
  // CODE CŨ (giữ lại tham khảo, đừng xóa):
  //   policy => policy.WithOrigins("http://localhost:3000")
  options.AddPolicy("AllowReactApp",
    policy => policy.WithOrigins(
                      (builder.Configuration["FRONTEND_URL"] ?? "http://localhost:3000")
                        .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    )
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader());
});

// 2. Đăng ký Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
builder.Services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


// 3. Đăng ký Service 
// (Vì IUserService kế thừa IServiceScoped nên hệ thống DI sẽ hiểu)
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMovieService, MockMovieService>();
builder.Services.AddScoped<ICinemaService, CinemaService>();
builder.Services.AddScoped<IShowtimeService, MockShowtimeService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Chuyển sang dùng postgres
// builder.Services.AddDbContext<TestContext>(options =>
//     options.UseSqlServer(
//         "Server=DESKTOP-76A1KAF\\SQLEXPRESS;Database=test;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddDbContext<TestContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// SỬA-DEPLOY: tự tạo bảng DB khi container khởi động (dotnet ef migrations đã có sẵn trong code).
// Để làm gì: DB postgres mới (volume pgdata mới / Neon mới) đang trống -> API sẽ lỗi 500 nếu không migrate.
// Bọc try/catch để nếu DB chưa sẵn sàng thì app vẫn chạy, log lỗi ra rồi tự retry ở lần deploy sau.
// CODE CŨ: trước đây không có đoạn này, phải chạy tay "dotnet ef database update".
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TestContext>();
    db.Database.Migrate();
    app.Logger.LogInformation("SỬA-DEPLOY: Database.Migrate() OK");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "SỬA-DEPLOY: Database.Migrate() thất bại (DB có thể chưa sẵn sàng, app vẫn chạy)");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// SỬA-DEPLOY: cho phép bật Swagger ở Production khi debug qua Tunnel bằng env ENABLE_SWAGGER=true.
// Để làm gì: mặc định Production tắt Swagger, lúc public URL lỗi thì không có chỗ test nhanh /api.
// CODE CŨ: không có đoạn này (chỉ có if IsDevelopment ở trên).
if (!app.Environment.IsDevelopment() && string.Equals(builder.Configuration["ENABLE_SWAGGER"], "true", StringComparison.OrdinalIgnoreCase))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// FIX Docker: CORS chỉ gọi 1 lần, phải đặt TRƯỚC MapHub/MapControllers.
// Lỗi cũ: policy "AllowNextJs" chưa từng được AddPolicy (chỉ có "AllowReactApp") -> crash startup.
// Giữ lại dòng cũ để tham khảo:
// app.UseCors("AllowNextJs");
app.UseCors("AllowReactApp");

app.MapHub<ChatHub>("/chatHub");
app.MapHub<ChatHub_v2>("/chatHub-v2");

// FIX Docker: dòng cũ đặt UseCors sau MapHub nên SignalR không nhận header CORS, giữ lại để tham khảo:
// app.UseCors("AllowReactApp");

// FIX Docker: trong container không có cert HTTPS -> bật redirect sẽ gây loop 307.
// Giữ dòng cũ để tham khảo:
// app.UseHttpsRedirection();
// Chỉ redirect ở Production có cert, còn Development (local + docker test) thì tắt:
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

// Code mẫu template cũ, giữ lại, hiện không dùng tới:
var summaries = new[]
{
    "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Sweltering","Scorching"
};

app.Run();
