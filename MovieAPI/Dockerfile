# Use .NET SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the csproj file and restore dependencies
COPY MovieAPI/MovieAPI.csproj MovieAPI/
WORKDIR /app/MovieAPI
RUN dotnet restore

# Copy the rest of the application code (destination must be a directory)
COPY MovieAPI/ ./

WORKDIR /app/MovieAPI

# Build and publish
RUN dotnet publish -c Release -o out

# Use .NET runtime to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published output correctly
COPY --from=build /app/MovieAPI/out/ /app/

# Expose port 5001
EXPOSE 5001

# Set the entrypoint for the API
ENTRYPOINT ["dotnet", "MovieAPI.dll"]
