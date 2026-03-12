# Raffle Management System - Client

The frontend application for the Raffle Management System, built with Angular 20 and modern UI component libraries.

## Table of Contents

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [UI Screenshots](#ui-screenshots)

## Overview

This is the client-side application of the Raffle Management System. It provides an intuitive, responsive user interface for managing raffle events, browsing gifts, making purchases, and administering the system based on user roles.

<img src="./screenshots/גיף כל האתר.gif" width="900" />


The application is built with Angular and uses a component-based architecture with lazy loading for optimal performance.

## Technologies Used

### Core Framework

- **Angular**: 20.3.0
- **TypeScript**: Latest


### UI Component Libraries

- **PrimeNG**: 20.4.0 - Primary UI component library
- **PrimeIcons**: 7.0.0 - Icon library
- **@primeng/themes**: 20.4.0 - Theme system
- **@primeuix/themes**: 2.0.2 - Extended themes

### Additional Libraries

- **canvas-confetti**: 1.9.4 - Celebration animations
- **file-saver**: 2.0.5 - File download functionality
- **jwt-decode**: 4.0.0 - JWT token handling
- **xlsx**: 0.18.5 - Excel export functionality

### Styling

- **SCSS**: For component and global styling
- Prettier configured for consistent code formatting

### Build Tools

- **Angular CLI**: 20.3.8
- **@angular/build**: 20.3.8


## Getting Started

Ensure these are installed on your development machine:

- **Node.js**: 22.16.0 or higher
- **npm**: 10.9.2 or higher
- **Angular CLI**: 20.3.x (optional for global commands)

Verify versions:

```bash
node --version
npm --version
ng version
```

## Getting Started (Client)

1. Clone the repository and open the client folder:

```bash
cd finalProject/client
```

2. Install dependencies:

```bash
npm install
```

3. Point the Angular app to the backend API


4. Start the dev server:

```bash
npm start
# or
ng serve --open
```

The app will open at `http://localhost:4200` by default.

5. Build for production:

```bash
npm run build
```

For backend details and database configuration, see the Server README: [../server/README.md](../server/README.md)

### 5. Access the Application

Open your browser and navigate to:

```
http://localhost:4200
```

**Note**: Ensure the backend server is running at the configured API endpoint before using the application.

## Project Structure

```
client/
├── public/                  # Static assets served directly
├── src/
│   ├── app/                # Main application directory
│   │   ├── core/          # Core functionality (guards, interceptors, services)
│   │   ├── features/      # Feature modules (lazy-loaded)
│   │   ├── shared/        # Shared components, directives, pipes
│   │   ├── styles/        # Application-wide styles
│   │   ├── app.config.ts  # Application configuration
│   │   ├── app.routes.ts  # Application routing
│   │   └── app.ts         # Root component
│   ├── index.html         # Main HTML file
│   ├── main.ts           # Application entry point
│   └── styles.scss       # Global styles
├── angular.json          # Angular CLI configuration
├── package.json          # Project dependencies and scripts
├── tsconfig.json         # TypeScript configuration
└── README.md            # This file
```

### Key Directories

- **core/**: Contains singleton services, authentication guards, HTTP interceptors, and core utilities that are used throughout the application
- **features/**: Contains feature-specific modules that are lazy-loaded for better performance
- **shared/**: Contains reusable components, directives, and pipes that are shared across features
- **styles/**: Contains global SCSS variables, mixins, and theme configurations


## UI Screenshots

This section showcases the user interface of the Raffle Management System.

### Home Page
<img src="./screenshots/home_page.png" width="660" />


### Gift Catalog
<img src="./screenshots/catalog.png" width="660" />

### Shopping Cart
<img src="./screenshots/cart.png" width="660" />

### Admin Panel
<img src="./screenshots/גיף%20מנהל.gif" width="660" />

### Add gift form
<img src="./screenshots/גיף%20הוספת%20מתנה.gif" width="660" />

### Donor Dashbord
<img src="./screenshots/גיף%20תורם.gif" width="660" />

### Authentication
<img src="./screenshots/register.png" width="660" />
<img src="./screenshots/login.png" width="660" />