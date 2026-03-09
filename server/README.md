# Raffle Management System - Server

The backend REST API for the Raffle Management System, built with ASP.NET Core 8.0 and Entity Framework Core.

## Table of Contents

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Overview](#api-overview)
- [Database](#database)
- [Authentication & Authorization](#authentication--authorization)
- [Logging](#logging)
- [Testing](#testing)

---

## Overview

This is the server-side REST API of the Raffle Management System. It provides secure endpoints for managing users, gifts, categories, purchases, and raffle draws with role-based authorization.

The API is built using a clean architecture approach with separation of concerns:
- **Controllers** handle HTTP requests and responses
- **Services** contain business logic
- **Repositories** manage data access
- **DTOs** define data transfer objects for API contracts
- **Middlewares** handle cross-cutting concerns

---

## Technologies Used

### Core Framework

- **ASP.NET Core**: 8.0
- **C#**: Latest with nullable reference types enabled
- **.NET SDK**: 8.0

### Database & ORM

- **Entity Framework Core**: 8.0.7
- **SQL Server**: Database provider
- **EF Core Tools**: For migrations and database management

### Authentication & Security

- **JWT Bearer Authentication**: `Microsoft.AspNetCore.Authentication.JwtBearer` (used for issuing and validating JWT tokens)
- **Password hashing**: `BCrypt.Net-Next` (used for secure password hashing)


---


## Getting Started

Follow these steps to configure and run the backend API locally.

## 1. Initial Setup
Clone the repository and navigate to the server directory:
```bash
cd server
```


## 2. Configuration
Configure the database connection and secrets

Edit `appsettings.Development.json`. Below is a complete example that matches the project's configuration fields — sensitive values are redacted. Do NOT commit real secrets to source control; use User Secrets or environment variables for secrets.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "Admin": {
    "Password": "<ADMIN_PASSWORD>"
  },

  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RaffleDb;User Id=sa;Password=<DB_PASSWORD>;TrustServerCertificate=True"
  },

  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSSL": true,
    "Username": "<EMAIL_USERNAME>",
    "Password": "<EMAIL_APP_PASSWORD>",
    "AdminEmail": "<ADMIN_EMAIL>"
  },

  "AllowedHosts": "*",

  "Jwt": {
    "Key": "<JWT_SECRET_KEY>",
    "Issuer": "CA-Api",
    "Audience": "CA-Client",
    "ExpiresMinutes": 60
  }
}
```

**Note**: For Gmail, use an [App Password](https://support.google.com/accounts/answer/185833) instead of your regular password.

Important: Ensure you update the connection string, admin password, and email settings before proceeding to the next steps.



## 3. Restore and Update
Run the following commands to install dependencies and sync your database schema:

```bash
dotnet restore
```

Apply Entity Framework Core migrations to create/update the database schema:

```bash
dotnet ef database update
```

## 4. Run the Application

```bash
dotnet run
```

The API will listen on the configured ports (check `launchSettings.json`).

## 5. API Documentation
Open the Swagger UI (e.g., `http://localhost:5000/swagger`) to explore endpoints.

---


## Project Structure

```
server/
├── Controllers/              # API endpoint controllers
│   ├── AuthController.cs    # Authentication endpoints
│   ├── CartController.cs    # Shopping cart management
│   ├── CategoryController.cs # Category CRUD
│   ├── DonorController.cs   # Donor-specific operations
│   ├── EmailController.cs   # Email notifications
│   ├── GiftController.cs    # Gift CRUD and queries
│   ├── PurchaseController.cs # Purchase management
│   ├── UserController.cs    # User management
│   └── WinningController.cs # Raffle draw and winners
│
├── Models/                   # Domain models (entities)
│   ├── UserModel.cs         # User entity
│   ├── GiftModel.cs         # Gift entity
│   ├── CategoryModel.cs     # Category entity
│   ├── PurchaseModel.cs     # Purchase entity
│   ├── WinningModel.cs      # Winning entity
│   └── enum/               # Enumerations
│
├── DTOs/                     # Data Transfer Objects
│   ├── AuthDto.cs           # Authentication DTOs
│   ├── CartDto.cs           # Cart DTOs
│   ├── CategoryDto.cs       # Category DTOs
│   ├── DonorDto.cs          # Donor DTOs
│   ├── GiftDto.cs           # Gift DTOs
│   ├── PurchaseDto.cs       # Purchase DTOs
│   ├── UserDto.cs           # User DTOs
│   └── WinningDto.cs        # Winning DTOs
│
├── Services/                 # Business logic layer
│   ├── Interfaces/          # Service interfaces
│   └── Implementations/     # Service implementations
│
├── Repositories/             # Data access layer
│   ├── Interfaces/          # Repository interfaces
│   └── Implementations/     # Repository implementations
│
├── Data/                     # Database context
│   ├── AppDbContext.cs      # EF Core DbContext
│   └── AppDbContextFactory.cs # Design-time factory
│
├── Mappings/                 # AutoMapper profiles
│   ├── CategoryProfile.cs
│   ├── DonorProfile.cs
│   ├── GiftProfile.cs
│   ├── PurchaseProfile.cs
│   ├── UserProfile.cs
│   └── WinningProfile.cs
│
├── Middlewares/              # Custom middleware
│   ├── ExceptionHandlingMiddleware.cs
│   └── ExceptionHandlingMiddlewareExtensions.cs
│
├── Migrations/               # EF Core migrations
├── Logs/                     # Application logs (auto-generated)
├── wwwroot/                  # Static files
├── Program.cs                # Application entry point
├── appsettings.json          # Production configuration
├── appsettings.Development.json # Development configuration
└── server.csproj             # Project file
```


---


## API Overview

The API follows RESTful conventions and is organized into the following resource groups:

## Authentication

| Endpoint | Description |
|----------|-------------|
| `POST /api/auth/login` | Authenticates a user by email and password, returns a JWT token and user details |
| `POST /api/auth/register` | Creates a new user with Role=User, returns a JWT token and user details |



## Users

| Endpoint | Description |
|----------|-------------|
| `GET /api/user` | Returns all users in the system — Admin only |
| `GET /api/user/{id}` | Returns a user by ID |
| `POST /api/user` | Creates a new user |
| `PUT /api/user/{id}` | Updates user details (name, email, phone, city, address, password, role) |
| `DELETE /api/user/{id}` | Deletes a user — Admin only |



## Gifts

| Endpoint | Description |
|----------|-------------|
| `GET /api/gift` | Returns all gifts with optional filtering via `?sort=`, `?categoryId=`, `?donorId=` |
| `GET /api/gift/all` | Returns all gifts with price sorting only |
| `GET /api/gift/{id}` | Returns a gift by ID |
| `GET /api/gift/byCategory/{categoryId}` | Returns all gifts belonging to a specific category |
| `GET /api/gift/byDonor/{donorId}` | Returns all gifts donated by a specific donor |
| `GET /api/gift/purchaseCount` | Returns the number of tickets purchased per gift |
| `POST /api/gift` | Creates a new gift with an image (multipart/form-data) — Admin only |
| `PUT /api/gift/{id}` | Updates an existing gift, including optional image replacement — Admin only |
| `DELETE /api/gift/{id}` | Deletes a gift — fails if associated purchases exist — Admin only |



## Categories

| Endpoint | Description |
|----------|-------------|
| `GET /api/category` | Returns all categories |
| `GET /api/category/{id}` | Returns a category by ID |
| `POST /api/category` | Creates a new category — Admin only |
| `PUT /api/category/{id}` | Updates a category name — Admin only |
| `DELETE /api/category/{id}` | Deletes a category — fails if associated gifts exist — Admin only |



## Cart

| Endpoint | Description |
|----------|-------------|
| `GET /api/cart/cart` | Returns the current user's cart (purchases with Draft status) |
| `POST /api/cart` | Adds a gift to the cart with a specified quantity — blocked if raffle has ended |
| `PUT /api/cart` | Updates the quantity of a cart item — creates a new item if it doesn't exist |
| `DELETE /api/cart/{purchaseId}` | Removes an item from the cart — only allowed if status is Draft |
| `POST /api/cart/checkout` | Converts all cart items from Draft to Completed |



## Purchases

| Endpoint | Description |
|----------|-------------|
| `GET /api/purchase` | Returns all purchases in the system — Admin only |
| `GET /api/purchase/{id}` | Returns a purchase by ID |
| `GET /api/purchase/byGift/{giftId}` | Returns all purchases for a specific gift — Admin only |
| `GET /api/purchase/count-by-gift` | Returns purchase count statistics grouped by gift — Admin only |
| `POST /api/purchase` | Creates a new purchase for the authenticated user |
| `PUT /api/purchase` | Updates the quantity and/or status of a purchase |
| `DELETE /api/purchase/{id}` | Deletes a purchase by ID |



## Winnings

| Endpoint | Description |
|----------|-------------|
| `GET /api/winning` | Returns all winnings — Admin only |
| `GET /api/winning/{id}` | Returns a winning entry by ID — Admin only |
| `GET /api/winning/doRaffle` | Runs a full raffle for all gifts without a winner, sends emails to winners and donors, and marks the raffle as closed — Admin only |
| `GET /api/winning/raffle-single/{giftId}` | Runs a raffle for a single gift and sends an email to the winner and donor — Admin only |
| `GET /api/winning/total-income` | Calculates the total income from all Completed purchases — Admin only |
| `GET /api/winning/statusIsFinished` | Returns whether the raffle has ended (true/false) — Admin only |
| `GET /api/winning/sorted-by-most-purchased` | Returns winnings sorted by the most purchased gift |
| `GET /api/winning/search` | Searches winnings by `?giftName=`, `?donorName=`, `?minPurchases=` |
| `POST /api/winning` | Manually adds a winning entry by GiftId and WinnerId — Admin only |
| `POST /api/winning/finishRaffle` | Marks the raffle as closed (blocks new cart additions) — Admin only |
| `POST /api/winning/resetStatus` | Resets the raffle status to Open — Admin only |
| `PUT /api/winning/{id}` | Updates an existing winning entry — Admin only |
| `DELETE /api/winning/{id}` | Deletes a winning entry — Admin only |



## Donors

| Endpoint | Description |
|----------|-------------|
| `GET /api/donor` | Returns all donors with optional filtering via `?search=` (name/email/phone) and `?city=` — Admin only |
| `GET /api/donor/with-gifts` | Returns all donors along with their donated gifts — Admin only |
| `GET /api/donor/dashboard` | Returns a dashboard for the authenticated donor: gift count, tickets sold, unique buyers, and winning status per gift — Donor only |
| `GET /api/donor/details` | Returns the profile details of the authenticated donor — Donor only |
| `POST /api/donor` | Creates a new donor with an encrypted password and Role=Donor — Admin only |
| `PATCH /api/donor/role/{userId}` | Changes the role of a specific user — Admin only |



## Email

| Endpoint | Description |
|----------|-------------|
| `POST /api/email/send-mail?giftId=&winnerId=` | Sends a winning email to both the winner and the donor with gift and raffle details — Admin only |


**Note**: Detailed API documentation with request/response schemas is available via Swagger UI at `/swagger`.


---


## Database

### Entities

The database includes the following main entities:

- **Users**: User accounts with roles (Admin, Donor, User)
- **Gifts**: Raffle gifts/prizes
- **Categories**: Gift categories
- **Purchases**: User purchases (tickets)
- **Winnings**: Raffle winners
- **Cart**: Shopping cart items

### Seed Data

A SQL script with sample data is available at [`documents/script.sql`](documents/script.sql).  
You can run it after applying the EF Core migrations to populate the database with initial data.

---

## Authentication & Authorization

### JWT Authentication

The API uses JSON Web Tokens (JWT) for stateless authentication:

1. User logs in via `POST /api/auth/login`
2. Server validates credentials and returns a JWT token
3. Client includes the token in the `Authorization` header for subsequent requests:
   ```
   Authorization: Bearer <token>
   ```

### Authorization Roles

Three roles are defined:

- **Admin** (Role = 0): Full system access
- **Donor** (Role = 1): Can see their own gifts
- **User** (Role = 2): Can browse and purchase

### Protected Endpoints

Endpoints are protected using the `[Authorize]` attribute with role-based policies:

```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(int id)
```

---

## Logging

The application uses **Serilog** for structured logging.

### Log Destinations

- **Console**: Real-time log output during development
- **File**: Logs are written to `Logs/log-YYYYMMDD.txt`
  - Rolling interval: Daily
  - Retention: 14 days

### Log Levels

- **Information**: General application flow
- **Warning**: Unusual situations that don't cause errors
- **Error**: Errors and exceptions
- **Debug**: Detailed diagnostic information (development only)

### Viewing Logs

Log files are located in the `Logs/` directory. Each day creates a new log file.

### HTTP Request Logging

All HTTP requests are automatically logged with:
- HTTP method and path
- Response status code
- Duration

---

## Testing

### Unit Tests

Unit tests are located in the `SERVER.Tests/` project.


### Architecture Principles

1. **Separation of Concerns**: Keep controllers thin, move logic to services
2. **Dependency Injection**: Register services in `Program.cs`
3. **Repository Pattern**: All data access through repositories
4. **DTOs**: Never expose domain models directly in APIs
5. **AutoMapper**: Use for mapping between models and DTOs


### Error Handling

The application uses global exception handling middleware:

- All exceptions are caught and logged
- Appropriate HTTP status codes are returned
- Error details are included in development mode only
- Generic error messages in production for security

