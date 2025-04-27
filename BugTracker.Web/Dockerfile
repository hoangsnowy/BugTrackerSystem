# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only .csproj to restore dependencies
COPY BugTracker.Web/*.csproj ./BugTracker.Web/
COPY BugTracker.Data/*.csproj ./BugTracker.Data/

# Restore
WORKDIR /src/BugTracker.Web
RUN dotnet restore

# Copy full source code separately
COPY BugTracker.Web/. ./BugTracker.Web/
COPY BugTracker.Data/. ./BugTracker.Data/

# ❗ Cực kỳ quan trọng: remove bin/obj nếu có
RUN rm -rf ./BugTracker.Web/bin ./BugTracker.Web/obj
RUN rm -rf ./BugTracker.Data/bin ./BugTracker.Data/obj

# Publish Web App
WORKDIR /src/BugTracker.Web
RUN dotnet publish "BugTracker.Web.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "BugTracker.Web.dll"]