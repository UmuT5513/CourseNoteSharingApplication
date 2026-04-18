# CourseNoteSharingSystem

Course notes sharing web app built with ASP.NET Core and EF Core.

## Stack

- .NET 10
- ASP.NET Core MVC + Razor Views
- Entity Framework Core
- SQL Server

## Features

- CRUD for **Notes**, **Courses**, and **Users**
- Layered MVC flow: `Controller -> DbContext -> View`
- Authentication progress: **Sign up completed**, next step is **Sign in**

## Key Files

- `CourseNoteSharingSystem/Program.cs` – app startup and middleware
- `CourseNoteSharingSystem/Data/CourseNoteSharingSystemContext.cs` – EF Core context
- `CourseNoteSharingSystem/Models/` – domain models
- `CourseNoteSharingSystem/Controllers/` – MVC controllers
- `CourseNoteSharingSystem/Views/` – Razor views
- `CourseNoteSharingSystem/appsettings.json` – configuration and `SqlCon`

## Run

1. Set `SqlCon` in `CourseNoteSharingSystem/appsettings.json`.
2. Build and run the project.
3. Open `/` (`Home/Index`).
