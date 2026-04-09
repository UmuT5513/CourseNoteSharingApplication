# CourseNoteSharingSystem

A simple ASP.NET Core web application for sharing course notes.

## Tech Stack

- .NET 10
- ASP.NET Core MVC with Razor Views
- Entity Framework Core
- SQL Server
- Bootstrap + jQuery validation

## Project Structure

- `CourseNoteSharingSystem/Program.cs`  
  Application startup, service registration, middleware, and route configuration.

- `CourseNoteSharingSystem/Data/CourseNoteSharingSystemContext.cs`  
  EF Core `DbContext` with `DbSet`s for:
  - `Note`
  - `Course`
  - `User`

- `CourseNoteSharingSystem/Models/`  
  Domain models:
  - `Note`
  - `Course`
  - `User`
  - `ErrorViewModel`

- `CourseNoteSharingSystem/Controllers/`  
  MVC controllers for application logic and CRUD flows:
  - `HomeController`
  - `NotesController`
  - `CoursesController`
  - `UsersController`

- `CourseNoteSharingSystem/Views/`  
  Razor views grouped by controller (`Notes`, `Courses`, `Users`, `Home`) plus shared layout files.

- `CourseNoteSharingSystem/wwwroot/`  
  Static assets (CSS, JavaScript, libraries).

- `CourseNoteSharingSystem/appsettings.json`  
  Configuration values including SQL Server connection string (`SqlCon`).

## What the App Does

The app provides basic CRUD operations for:

- Notes
- Courses
- Users

Typical flow:

`Request -> Controller -> DbContext -> View -> Response`

## Run Locally

1. Update the `SqlCon` connection string in `CourseNoteSharingSystem/appsettings.json` if needed.
2. Build and run the project in Visual Studio.
3. Open the app URL and use the default route:
   - `/` -> `Home/Index`
