# ============================================================================
# DOCKERFILE cho project MyApi (.NET 8 Web API)
# Cách dùng:
#   1. Đặt file này tên là "Dockerfile" (không có đuôi) ở cùng thư mục với MyApi.csproj
#   2. Build:  docker build -t myapi .
#   3. Chạy:   docker run -p 5102:8080 --name myapi-container myapi
#   4. Mở trình duyệt: http://localhost:5102/swagger
# ============================================================================

# ----------------------------------------------------------------------------
# STAGE 1: "base" - Nền để CHẠY app (runtime)
# ----------------------------------------------------------------------------
# - Ảnh "aspnet:8.0" chỉ chứa những gì cần để CHẠY app .NET (không có compiler).
#   Nó nhẹ hơn ảnh "sdk" rất nhiều -> giúp container cuối cùng nhẹ, bảo mật hơn.
# - Đặt tên stage là "base" để stage cuối cùng tái sử dụng lại (FROM base).
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# - WORKDIR: tạo và chuyển vào thư mục làm việc /app bên trong container.
#   Mọi lệnh COPY, RUN, ENTRYPOINT sau đó đều chạy trong /app.
WORKDIR /app

# - EXPOSE chỉ mang tính "ghi chú / tài liệu": báo cho người đọc biết
#   container sẽ lắng nghe ở cổng nào. Nó KHÔNG tự mở cổng ra máy thật.
#   Muốn truy cập từ máy thật vẫn phải dùng "-p 5102:8080" lúc docker run.
# - Từ .NET 8, ảnh aspnet mặc định chạy ở cổng 8080 (http) và 8081 (https),
#   KHÔNG còn 80/443 như .NET 6/7 nữa.
EXPOSE 8080
EXPOSE 8081

# ----------------------------------------------------------------------------
# STAGE 2: "build" - Môi trường để BIÊN DỊCH app (SDK)
# ----------------------------------------------------------------------------
# - Ảnh "sdk:8.0" chứa đầy đủ: compiler, dotnet restore, dotnet build, dotnet publish.
#   Ảnh này nặng (~800MB) nên ta chỉ dùng để build, rồi bỏ đi ở stage cuối.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# - Tham số build: cho phép đổi Configuration lúc build.
#   Mặc định là Release (tối ưu, nhanh, nhẹ). Lúc debug có thể build Debug.
#   Dùng: docker build --build-arg BUILD_CONFIGURATION=Debug -t myapi .
ARG BUILD_CONFIGURATION=Release

# - Thư mục /src là nơi ta sẽ copy source code vào để build (quy ước chung).
WORKDIR /src

# - Copy MỘT MÌNH file .csproj vào trước, rồi chạy "dotnet restore" ngay.
#   Đây là mẹo CACHE của Docker:
#   + Docker cache từng dòng lệnh theo từng layer.
#   + Nếu bạn chỉ sửa code .cs mà không đổi thư viện (csproj), Docker sẽ
#     dùng lại layer restore cũ -> build nhanh hơn rất nhiều.
#   + Nếu COPY hết cả project ngay từ đầu, chỉ cần sửa 1 file là phải
#     restore lại toàn bộ -> rất chậm.
COPY ["MyApi.csproj", "./"]
RUN dotnet restore "MyApi.csproj"

# - Giờ mới copy TOÀN BỘ source còn lại (Controllers/, Services/, Program.cs...)
#   vào /src trong container.
COPY . .

# - Chuyển vào thư mục /src (cho chắc) rồi build project.
#   -c $BUILD_CONFIGURATION : build theo cấu hình Release/Debug ở trên.
#   -o /app/build           : xuất kết quả build trung gian ra /app/build.
WORKDIR "/src"
RUN dotnet build "MyApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ----------------------------------------------------------------------------
# STAGE 3: "publish" - Đóng gói app để mang sang môi trường chạy
# ----------------------------------------------------------------------------
# - Kế thừa từ stage "build" nên có sẵn code đã build, không cần restore lại.
# - "dotnet publish" sẽ: biên dịch Release + copy DLL + appsettings.json +
#   các file cần thiết để chạy -> gọn nhẹ, bỏ hết source .cs, obj/, bin/ thừa.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ----------------------------------------------------------------------------
# STAGE 4: "final" - Ảnh CUỐI CÙNG, nhẹ, dùng để chạy thật
# ----------------------------------------------------------------------------
# - Quay lại dùng ảnh "base" (aspnet runtime nhẹ), KHÔNG mang theo SDK nặng.
# - Đây là kỹ thuật "multi-stage build": vứt bỏ hết đồ nghề build,
#   chỉ copy thành phẩm từ stage publish sang.
FROM base AS final

WORKDIR /app

# - Copy toàn bộ thư mục /app/publish ở stage "publish" sang /app hiện tại.
#   Lúc này /app chỉ chứa: MyApi.dll, appsettings.json, web.config...
COPY --from=publish /app/publish .

# - ENTRYPOINT: lệnh mặc định chạy khi container khởi động.
#   Ở đây là "dotnet MyApi.dll" = chạy Web API của bạn.
#   Dùng dạng mảng JSON ["dotnet", "MyApi.dll"] thay vì dạng chuỗi
#   để tín hiệu dừng (Ctrl+C, docker stop) truyền đúng vào app.
ENTRYPOINT ["dotnet", "MyApi.dll"]
