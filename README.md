# Campus Activity Hub

A web application for managing campus activities, events, and registrations.

## Prerequisites

Before running the application locally, ensure you have the following installed:

- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Git** - [Download](https://git-scm.com/)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd StepProject
```

### 2. Navigate to the Project Directory

```bash
cd CampusActivityHub
```

### 3. Restore NuGet Packages

Install all required dependencies:

```bash
dotnet restore
```

### 4. Set Up the Database

The application uses SQLite for the database. Run the Entity Framework migrations to create the database:

```bash
dotnet ef database update
```

This will create a `CampusHub.db` file in the project root directory.

### 5. Run the Application

Start the development server:

```bash
dotnet run
```

The application will start and display output similar to:
```
Now listening on: https://localhost:7015
Now listening on: http://localhost:5292
```

### 6. Access the Website

Open your web browser and navigate to:
- **HTTPS**: `https://localhost:7015`
- **HTTP**: `http://localhost:5292`

The application will load with the default route pointing to the Events page.

## Project Structure

- **Controllers/** - MVC controllers for handling requests
- **Models/** - Data models (Event, User, Category, Registration, Tag)
- **Views/** - Razor views for the UI
- **Data/** - Database context and initialization
- **Services/** - Business logic (EmailService, etc.)
- **Migrations/** - Database migration files
- **wwwroot/** - Static files (CSS, JavaScript, libraries)

## Configuration

Configuration settings are stored in `appsettings.json`:

- **DefaultConnection** - SQLite database path (defaults to `CampusHub.db`)
- **Logging** - Logging level configuration

### Development Configuration

For development-specific settings, use `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CampusHub.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Build and Release

### Build for Development

```bash
dotnet build
```

### Build for Production

```bash
dotnet build -c Release
```

### Publish for Deployment

```bash
dotnet publish -c Release -o ./publish
```

## Troubleshooting

### Database Issues

If you encounter database errors, you can reset the database:

```bash
rm CampusHub.db
dotnet ef database update
```

### Port Already in Use

If the default ports are already in use, you can specify a different port:

```bash
dotnet run --urls "https://localhost:7016;http://localhost:5293"
```

### SSL Certificate Issues

For local development with HTTPS, you may need to trust the development certificate:

```bash
dotnet dev-certs https --trust
```

## Features

- Event management and creation
- User registration and authentication
- Event categorization and tagging
- Admin dashboard
- Email notifications
- Request logging

## Technologies Used

- **Framework**: ASP.NET Core 8.0
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Razor Views, Bootstrap, jQuery

## Support

For issues or questions, please refer to the project documentation or contact the development team.
