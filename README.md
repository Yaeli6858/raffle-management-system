# Raffle Management System

A comprehensive full-stack web application for managing raffle events with role-based access control. The system enables administrators to manage gifts and draw winners, donors to contribute gifts, and users to participate in raffles by purchasing tickets.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Contributing](#contributing)

For detailed information about each component:
- See [client/README.md](client/README.md) for frontend documentation
- See [server/README.md](server/README.md) for backend documentation

## Overview

The Raffle Management System is designed to streamline the process of organizing and managing raffle events. It provides a modern, intuitive interface for users to browse available gifts, manage their shopping cart, and participate in raffles, while giving administrators and donors powerful tools to manage the entire raffle lifecycle.

### User Roles

- **Administrator**: Manages the entire system, including users, gifts, categories, and raffle draws
- **Donor**: Contributes gifts to the raffle system
- **User**: Browses gifts, purchases raffle tickets, and participates in raffles

## Features

- **Authentication & Authorization**: Secure JWT-based authentication with role-based access control
- **User Management**: Complete user registration, login, and profile management
- **Gift Management**: Create, update, and manage raffle gifts with images and categories
- **Category Organization**: Organize gifts into categories for easy browsing
- **Shopping Cart**: Add multiple gifts to cart before checkout
- **Purchase Management**: Process purchases with draft and completed statuses
- **Raffle Draws**: Automated system for selecting and managing winners
- **Email Notifications**: Automated email notifications for important events
- **Responsive Design**: Modern, mobile-friendly user interface
- **API Documentation**: Built-in Swagger UI for exploring and testing API endpoints

## Architecture

The application follows a modern three-tier architecture:

```
┌─────────────────┐
│  Angular Client │  (Frontend - Port 4200)
│   UI Layer      │
└────────┬────────┘
         │ HTTP/REST
         │
┌────────▼────────┐
│  ASP.NET Core   │  (Backend API - Port 5000)
│  REST API       │
└────────┬────────┘
         │ Entity Framework
         │
┌────────▼────────┐
│  SQL Server     │  (Database - Port 1433)
│   Database      │
└─────────────────┘
```

### Communication Flow

1. **Client ↔ Server**: The Angular frontend communicates with the ASP.NET Core backend via RESTful API calls
2. **Server ↔ Database**: The backend uses Entity Framework Core as an ORM to interact with SQL Server
3. **Authentication**: JWT tokens are issued by the server and included in subsequent client requests
4. **CORS**: Configured to allow the Angular development server to communicate with the API

## Tech Stack

### Frontend

- **Framework**: Angular 20.3
- **UI Libraries**: 
  - PrimeNG 20.4 (Primary component library)
- **Language**: TypeScript
- **Styling**: SCSS
- **Additional Libraries**: 
  - RxJS for reactive programming
  - JWT-decode for token handling
  - Canvas-confetti for celebrations
  - XLSX for Excel exports
  - File-saver for file downloads

### Backend

- **Framework**: ASP.NET Core 8.0
- **Language**: C#
- **ORM**: Entity Framework Core 8.0
- **Authentication**: JWT Bearer tokens
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI
- **Additional Libraries**:
  - AutoMapper for object mapping
  - BCrypt.Net for password hashing

### Database

- **DBMS**: SQL Server
- **Migration Tool**: Entity Framework Core Migrations

## Prerequisites

Before running this project, ensure you have the following installed:

- **Node.js**: 22.16.0 or higher
- **npm**: 10.9.2 or higher
- **Angular CLI**: 20.3.13 or higher
- **.NET SDK**: 8.0 or higher
- **SQL Server**: Local instance, Docker container, or remote server


### Verify Installation

```bash
# Check Node.js version
node --version

# Check npm version
npm --version

# Check Angular CLI version
ng version

# Check .NET SDK version
dotnet --version
```

## Getting Started

Follow these steps to run the application locally:

### 1. Clone the Repository

```bash
git clone <repository-url>
cd finalProject
```

### 2. Set Up the Database


#### Using Local SQL Server

Ensure SQL Server is running on your machine and accessible.

### 3. Configure the Server

Navigate to the server directory and update the connection string:

```bash
cd server
```

Edit `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RaffleDb;User Id=sa;Password=YourStrong@Pass123;TrustServerCertificate=True"
  }
}
```

**Important Configuration Items**:
- Update the `ConnectionStrings:DefaultConnection` with your database credentials
- Configure `EmailSettings` if you need email functionality
- The `Jwt` settings are pre-configured but can be customized if needed

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

### 5. Start the Server

```bash
dotnet run
```

The API will be available at `http://localhost:5000` (or the port specified in `launchSettings.json`).

Access the Swagger documentation at: `http://localhost:5000/swagger`

### 6. Set Up the Client

Open a new terminal and navigate to the client directory:

```bash
cd client
```

Install dependencies:

```bash
npm install
```

### 7. Start the Client

```bash
npm start
# or
ng serve
```

The application will be available at `http://localhost:4200`.

### 8. Access the Application

Open your browser and navigate to `http://localhost:4200`.

## Project Structure

```
finalProject/
├── client/               # Angular frontend application
│   ├── src/
│   │   ├── app/         # Application components and modules
│   │   ├── assets/      # Static assets
│   │   └── styles/      # Global styles
│   └── package.json     # Client dependencies
│
├── server/              # ASP.NET Core backend API
│   ├── Controllers/     # API endpoints
│   ├── Models/          # Domain models
│   ├── DTOs/            # Data transfer objects
│   ├── Services/        # Business logic layer
│   ├── Repositories/    # Data access layer
│   ├── Data/            # Database context
│   ├── Migrations/      # EF Core migrations
│   ├── Middlewares/     # Custom middleware
│   └── Program.cs       # Application entry point
│
├── SERVER.Tests/        # Unit tests
└── README.md           # This file
```



