FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY JobApplicationTracker.sln ./
COPY src/JobApplicationTracker.Api/JobApplicationTracker.Api.csproj src/JobApplicationTracker.Api/
COPY src/JobApplicationTracker.Application/JobApplicationTracker.Application.csproj src/JobApplicationTracker.Application/
COPY src/JobApplicationTracker.Domain/JobApplicationTracker.Domain.csproj src/JobApplicationTracker.Domain/
COPY src/JobApplicationTracker.Infrastructure/JobApplicationTracker.Infrastructure.csproj src/JobApplicationTracker.Infrastructure/

# Restore dependencies
RUN dotnet restore "src/JobApplicationTracker.Api/JobApplicationTracker.Api.csproj"

# Copy the rest of the source
COPY . .

# Publish the application
RUN dotnet publish "src/JobApplicationTracker.Api/JobApplicationTracker.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV HTTP_PORTS=8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 \
    ConnectionStrings__Default="Data Source=/app/data/job-application-tracker.db"

RUN mkdir -p /app/data

COPY --from=build /app/publish .

VOLUME ["/app/data"]

EXPOSE 8080

ENTRYPOINT ["dotnet", "JobApplicationTracker.Api.dll"]

