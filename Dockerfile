# Use the official ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Choresbuddy-dotnet/Choresbuddy-dotnet.csproj", "Choresbuddy-dotnet/"]
RUN dotnet restore "Choresbuddy-dotnet/Choresbuddy-dotnet.csproj"

# Copy the entire project and build it
COPY . .
WORKDIR "/src/Choresbuddy-dotnet"
RUN dotnet build "Choresbuddy-dotnet.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Choresbuddy-dotnet.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Choresbuddy-dotnet.dll"]
