# SecureAuth

SecureAuth is a secure authentication and authorization REST API built with ASP.NET Core.

The project is being developed as a portfolio project to demonstrate professional backend development practices, authentication security, API architecture, database design, testing, and GitHub workflow.

## 🚧 Project Setup
**Task**
* [x] Initial project architecture created
* [x] ASP.NET Core Web API initialized
* [x] Solution architecture configured
* [x] Environment configuration
* [x] Swagger/OpenAPI configured
* [x] Dependency injection configured
* [x] Required NuGet packages installed


## 🛠️ Technologies

* **.NET / ASP.NET Core**
* **Entity Framework Core**
* **SQL Server**
* **JWT**
* **Swagger / OpenAPI**
* **xUnit**
* **Git / GitHub**

## 🏗️ Project Architecture

SecureAuth follows a layered architecture that separates API concerns,
application logic, domain models, and infrastructure implementations.

```text
SecureAuth
│
├── SecureAuth.Api
│   ├── Controllers
│   ├── Middlewares
│   ├── Program.cs
│   ├── appsettings.json
│   └── SecureAuth.Api.http
│
├── SecureAuth.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Mappings
│   └── Validators
│
├── SecureAuth.Domain
│   ├── Entities
│   ├── Enums
│   └── Frameworks
│
├── SecureAuth.Infrastructure
│   ├── Persistence
│   ├── Repositories
│   ├── Services
│   └── Settings
│
└── SecureAuth.sln