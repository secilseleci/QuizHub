# Temel çalışma ortamını tanımlıyoruz (ASP.NET Core Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build işlemi için SDK ortamını tanımlıyoruz
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Proje dosyasını kopyalıyoruz
COPY ["QuizHubPresentation/QuizHubPresentation.csproj", "QuizHubPresentation/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Services/Services.csproj", "Services/"]

# Bağımlılıkları yüklüyoruz
RUN dotnet restore "QuizHubPresentation/QuizHubPresentation.csproj"

# Kaynak dosyalarını kopyalıyoruz
COPY . .
WORKDIR "/src/QuizHubPresentation"

# Projeyi build ediyoruz
RUN dotnet build -c Release -o /app/build

# Build edilmiş projeyi yayınlıyoruz
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final imajımızı oluşturuyoruz
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizHubPresentation.dll"]
