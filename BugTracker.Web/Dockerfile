FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY .editorconfig ./

# Copy solution and project files
COPY BugTracker.sln ./
COPY BugTracker.Data/*.csproj ./BugTracker.Data/
COPY BugTracker.Web/*.csproj ./BugTracker.Web/

# Restore (editorconfig đã ở đây, warning rules sẽ apply)
RUN dotnet restore "./BugTracker.Web/BugTracker.Web.csproj"

# Copy source code
COPY BugTracker.Data/. ./BugTracker.Data/
COPY BugTracker.Web/. ./BugTracker.Web/

# Clean up old artifacts
RUN rm -rf BugTracker.Data/bin BugTracker.Data/obj \
           BugTracker.Web/bin  BugTracker.Web/obj

# Publish
WORKDIR /src/BugTracker.Web
RUN dotnet publish "BugTracker.Web.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet","BugTracker.Web.dll"]