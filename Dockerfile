# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY AspNetCoreProject.csproj .
RUN dotnet restore

# Copy the rest of your application code
COPY . .

# Publish the application
RUN dotnet publish "AspNetCoreProject.csproj" -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose the port your ASP.NET Core app listens on (e.g., 80 for HTTP)
EXPOSE 80

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "AspNetCoreProject.dll"]