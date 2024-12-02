# Base image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["src/PaymentApi/PaymentApi.csproj", "PaymentApi/"]
RUN dotnet restore "PaymentApi/PaymentApi.csproj"

# Copy only the source code for PaymentApi
COPY src/PaymentApi/ ./PaymentApi/
WORKDIR "/src/PaymentApi"

# Build the main application
RUN dotnet build "PaymentApi.csproj" -c Release -o /app/build

# Publish the main application
FROM build AS publish
RUN dotnet publish "PaymentApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8580
ENTRYPOINT ["dotnet", "PaymentApi.dll"]
