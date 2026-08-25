# SecureAuth

SecureAuth is a secure authentication and authorization REST API built with ASP.NET Core.

The project is being developed as a portfolio project to demonstrate professional backend development practices, authentication security, API architecture, database design, testing, and GitHub workflow.

## 🚧 Project Setup

**Tasks**

* [x] Initial project architecture created
* [x] ASP.NET Core Web API initialized
* [x] Solution architecture configured
* [x] Environment configuration
* [x] Swagger/OpenAPI configured
* [x] Dependency injection configured
* [x] Required NuGet packages installed

---

## 2. Configure Entity Framework Core

**Tasks**

* [x] Add Entity Framework Core
* [x] Configure SQL Server
* [x] Create `ApplicationDbContext`
* [x] Configure EF Core migrations
* [x] Configure the database connection
* [x] Register `ApplicationDbContext` with dependency injection

---

## 3. Implement Authentication Core

**Tasks**

Implement the core authentication and email verification functionality for the SecureAuth ASP.NET Core Web API.

### 1. Create User Entity

* [x] Create the `User` entity
* [x] Define the required user properties
* [x] Configure the `User` entity in `ApplicationDbContext`

### 2. Create Role

* [x] Create the `Role` entity
* [x] Define role properties
* [x] Configure the role entity and relationships

### 3. Create Database Migration

* [x] Verify all authentication entities are correctly configured
* [x] Create the initial authentication migration
* [x] Review the generated migration
* [x] Apply the migration to SQL Server
* [x] Verify the database schema

### 4. Create Mail Service

* [x] Create a mail service abstraction
* [x] Create the mail service implementation
* [x] Use `MimeKit` and `MailKit` for creating and sending emails
* [x] Configure SMTP/email settings through application configuration
* [x] Register the mail service using dependency injection
* [x] Ensure sensitive email credentials are not hardcoded

### 5. Create OTP Verification

* [x] Create the OTP verification DTO
* [x] Validate OTP and email input using FluentValidation
* [x] Use the `Otp.NET` library for OTP generation and verification
* [x] Generate OTP codes securely
* [x] Store verification information securely
* [x] Configure OTP expiration
* [x] Create the verification email template
* [x] Send an OTP verification email after registration
* [x] Create the email verification endpoint
* [x] Validate the submitted OTP
* [x] Handle expired or invalid OTP codes
* [x] Mark the user's email as verified after successful verification

**Endpoint:**

```http
POST /api/auth/verify
```

### 6. Create Resend OTP Endpoint

* [x] Create the resend OTP request DTO
* [x] Validate the email input using FluentValidation
* [x] Check whether the user exists
* [x] Check the user's email verification status
* [x] Generate a new OTP code securely
* [x] Send a new verification email
* [x] Replace or invalidate the previous OTP

**Endpoint:**

```http
POST /api/auth/resend
```

### 7. Create Registration Endpoint

* [x] Create the registration request DTO
* [x] Validate registration input using FluentValidation
* [x] Check whether the email already exists
* [x] Hash the user's password securely
* [x] Create the user
* [x] Assign the default role
* [x] Generate an OTP verification code
* [x] Send the OTP verification email
* [x] Return an appropriate API response
* [x] Handle validation and duplicate-user errors correctly

**Endpoint:**

```http
POST /api/auth/register
```

### 8. Create Login Endpoint

* [x] Create the login request DTO
* [x] Validate login credentials using FluentValidation
* [x] Find the user by email
* [x] Verify the password securely
* [x] Check whether the account is active
* [x] Check email verification status
* [x] Generate the authentication token
* [x] Return the authentication response
* [x] Handle invalid credentials securely

**Endpoint:**

```http
POST /api/auth/login
```

### 9. Configure Auth Controller

* [x] Add the registration endpoint
* [x] Add the login endpoint
* [x] Add the resend OTP endpoint
* [x] Add the verify OTP endpoint
* [x] Delegate authentication logic to `IAuthService`
* [x] Return responses consistently using `HandleResult`

**Available Endpoints:**

```http
POST /api/auth/register
POST /api/auth/login
POST /api/auth/resend
POST /api/auth/verify
```

---

## 🛠️ Technologies

* **.NET / ASP.NET Core**
* **ASP.NET Core Identity**
* **Entity Framework Core**
* **SQL Server**
* **JWT Authentication**
* **FluentValidation**
* **Otp.NET**
* **MimeKit**
* **MailKit**
* **Swagger / OpenAPI**
* **xUnit**
* **Git / GitHub**

## 🏗️ Project Architecture

SecureAuth follows a layered architecture that separates API concerns, application logic, domain models, and infrastructure implementations.

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
```
