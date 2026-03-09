![Angular](https://img.shields.io/badge/Angular-20-DD0031?style=flat&logo=angular&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=flat&logo=typescript&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat&logo=microsoftsqlserver&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-black?style=flat&logo=jsonwebtokens)
![Serilog](https://img.shields.io/badge/Serilog-Logging-blue?style=flat)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)




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

For detailed information about each component:
- See [client/README.md](client/README.md) for frontend documentation
- See [server/README.md](server/README.md) for backend documentation

## Overview

The Raffle Management System is designed to streamline the process of organizing and managing raffle events. It provides a modern, intuitive interface for users to browse available gifts, manage their shopping cart, and participate in raffles, while giving administrators and donors powerful tools to manage the entire raffle lifecycle.

### User Roles

| Role | Description |
|------|-------------|
| **Administrator** | Manages the entire system, including users, gifts, categories, and raffle draws |
| **Donor** | Has view-only access to see data and statistics regarding the gifts they have donated |
| **User** | Browses gifts, purchases raffle tickets, and participates in raffles |

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

These steps provide a minimal, high-level flow to get the full system running locally. For detailed configuration and platform-specific options, follow the component READMEs linked below.

1. Configure the backend connection string and secrets (see Server README).
2. Start the backend: restore, apply EF Core migrations, and run the API (see Server README).
3. Install frontend dependencies and start the Angular development server (see Client README).
4. Open the application in your browser at `http://localhost:4200`.

For detailed instructions and environment-specific configuration:

- Server: [server/README.md](server/README.md)
- Client: [client/README.md](client/README.md)

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



 
