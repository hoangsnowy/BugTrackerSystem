FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy và restore đúng project
COPY ./BugTracker.Data ./BugTracker.Data
COPY ./BugTracker.Web ./BugTracker.Web
COPY ./BugTracker.sln ./
RUN dotnet restore "BugTracker.Web/BugTracker.Web.csproj"

# Build và publish
WORKDIR /src/BugTracker.Web
RUN dotnet publish "BugTracker.Web.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BugTracker.Web.dll"]