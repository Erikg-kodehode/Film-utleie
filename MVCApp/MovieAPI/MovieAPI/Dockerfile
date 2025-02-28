# Use .NET SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build the project and publish the output
RUN dotnet publish -c Release -o out

# Use .NET runtime for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published build from the previous stage
COPY --from=build /app/out ./

# Expose the port to allow external access
EXPOSE 5000

RUN apt update && apt install -y sqlite3


# Set the entrypoint for the API
ENTRYPOINT ["dotnet", "MovieAPI.dll"]
