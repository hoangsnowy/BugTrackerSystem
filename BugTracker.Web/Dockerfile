# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy editorconfig (to apply style rules)
COPY .editorconfig ./

# Copy solution and project files
COPY BugTracker.sln ./
COPY BugTracker.Data/*.csproj BugTracker.Data/
COPY BugTracker.Business/*.csproj BugTracker.Business/
COPY BugTracker.Web/*.csproj BugTracker.Web/

# Restore dependencies for Web project (pulls in Data & Business)
RUN dotnet restore "BugTracker.Web/BugTracker.Web.csproj"

# Copy source code
COPY BugTracker.Data/. BugTracker.Data/
COPY BugTracker.Business/. BugTracker.Business/
COPY BugTracker.Web/. BugTracker.Web/

# Clean old build artifacts
RUN rm -rf BugTracker.Data/bin BugTracker.Data/obj \
           BugTracker.Business/bin BugTracker.Business/obj \
           BugTracker.Web/bin  BugTracker.Web/obj

# Publish Web project
WORKDIR /src/BugTracker.Web
RUN dotnet publish "BugTracker.Web.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "BugTracker.Web.dll"]