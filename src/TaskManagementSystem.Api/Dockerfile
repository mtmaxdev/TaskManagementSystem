FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskManagementSystem.Api/TaskManagementSystem.Api.csproj", "TaskManagementSystem.Api/"]
COPY ["TaskManagementSystem.Application/TaskManagementSystem.Application.csproj", "TaskManagementSystem.Application/"]
COPY ["TaskManagementSystem.Domain/TaskManagementSystem.Domain.csproj", "TaskManagementSystem.Domain/"]
COPY ["TaskManagementSystem.Infrastructure/TaskManagementSystem.Infrastructure.csproj", "TaskManagementSystem.Infrastructure/"]
RUN dotnet restore "TaskManagementSystem.Api/TaskManagementSystem.Api.csproj"
COPY . .
WORKDIR "/src/TaskManagementSystem.Api"
RUN dotnet build "TaskManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskManagementSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementSystem.Api.dll"]
