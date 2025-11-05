# Motel API

A comprehensive REST API for motel management operations, built with ASP.NET Core 7 and Entity Framework. This API provides backend services for managing rooms, customers, and administrative operations for motel businesses.

## 🌟 Features

- **Room Management API**: CRUD operations for room inventory
- **Customer Management**: Customer registration and profile management
- **Database Management**: Entity Framework with SQL Server 2022
- **API Documentation**: Interactive Swagger/OpenAPI documentation
- **Repository Pattern**: Clean architecture with repository design pattern
- **Real-time Data**: Live synchronization with frontend applications

## 🚀 Live Demo

**API Documentation**: [motelapi.junnio.xyz/swagger/index.html](https://motelapi.junnio.xyz/swagger/index.html)

## 🏗️ Architecture

This API follows the Repository Design Pattern with:
- **Controllers**: Handle HTTP requests and responses
- **Services**: Business logic implementation
- **Repositories**: Data access layer abstraction
- **Models**: Entity definitions and DTOs
- **Database**: SQL Server 2022 with Entity Framework Code First

## 📋 Prerequisites

Before running this API, ensure you have:

- .NET 8.0 SDK or higher
- SQL Server 2022 (or SQL Server Express)
- Visual Studio 2022 or VS Code
- Postman or similar API testing tool (optional)

## 🔧 Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Junn1o/motelapi.git
   cd motelapi
   ```

2. **Configure the database connection**
   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "your_connection_string"
     }
   }
   ```

3. **Install dependencies**
   ```bash
   dotnet restore
   ```

4. **Setup the database**
   ```bash
   dotnet ef migrations add MigrationName
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7139` (HTTPS) and `http://localhost:5088` (HTTP)


### Base URL
- **Production**: `https://motelapi.junnio.xyz`
- **Development**: `https://localhost:7139`

## 🔗 Related Applications

This API serves as the backend for:

- **Admin Dashboard**: [Motel Admin Dashboard](https://github.com/Junn1o/MotelDB) - Management interface
- **Customer Frontend**: [Motel Room Client](https://github.com/Junn1o/motel-room) - Customer booking portal

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8
- **Database**: SQL Server 2022
- **ORM**: Entity Framework Core
- **Documentation**: Swagger/OpenAPI
- **Architecture**: Repository Pattern
- **Containerization**: Docker support
- **Language**: C#

## 🐳 Docker Support

Build and run with Docker:

   Environment Variables
   Create a `.env` file in the root directory:
   ```env
   ASPNETCORE_Kestrel__Certificates__Default__Password="your_password"
   ConnectionStrings__DefaultConnection="your_connection_string"
   Jwt__Key="your_key"
   Jwt__Issuer="your_issuer_url"
   Jwt__Audience="your_audience_url"
   ```
```bash
# Build image
docker build -t motel-api .

# Run container
docker run -p 9024:9024 motel-api

#or simply run with docker compose:
docker compose up -d
```
The API will be available at `https://localhost:9024` (HTTPS) and `http://localhost:9023` (HTTP)