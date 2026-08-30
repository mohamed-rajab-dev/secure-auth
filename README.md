# SecureAuth

SecureAuth is a secure authentication and authorization REST API built with ASP.NET Core.

The project is being developed as a portfolio project to demonstrate professional backend development practices, authentication security, authorization, API architecture, database design, testing, and GitHub workflow.

---

## 🚧 Project Setup

**Tasks**

- [x] Initial project architecture created
- [x] ASP.NET Core Web API initialized
- [x] Solution architecture configured
- [x] Environment configuration
- [x] Swagger/OpenAPI configured
- [x] Dependency injection configured
- [x] Required NuGet packages installed

---

## 2. Configure Entity Framework Core

**Tasks**

- [x] Add Entity Framework Core
- [x] Configure SQL Server
- [x] Create `ApplicationDbContext`
- [x] Configure EF Core migrations
- [x] Configure the database connection
- [x] Register `ApplicationDbContext` with dependency injection

---

## 3. Implement Authentication Core

Implement the core authentication and email verification functionality for the SecureAuth ASP.NET Core Web API.

### 1. Create User Entity

- [x] Create the `User` entity
- [x] Define the required user properties
- [x] Configure the `User` entity in `ApplicationDbContext`

### 2. Create Role

- [x] Create the `Role` entity
- [x] Define role properties
- [x] Configure the role entity and relationships

### 3. Create Database Migration

- [x] Verify all authentication entities are correctly configured
- [x] Create the initial authentication migration
- [x] Review the generated migration
- [x] Apply the migration to SQL Server
- [x] Verify the database schema

### 4. Create Mail Service

- [x] Create a mail service abstraction
- [x] Create the mail service implementation
- [x] Use `MimeKit` and `MailKit` for creating and sending emails
- [x] Configure SMTP/email settings through application configuration
- [x] Register the mail service using dependency injection
- [x] Ensure sensitive email credentials are not hardcoded

### 5. Create OTP Verification

- [x] Create the OTP verification DTO
- [x] Validate OTP and email input using FluentValidation
- [x] Use the `Otp.NET` library for OTP generation and verification
- [x] Generate OTP codes securely
- [x] Store verification information securely
- [x] Configure OTP expiration
- [x] Create the verification email template
- [x] Send an OTP verification email after registration
- [x] Create the email verification endpoint
- [x] Validate the submitted OTP
- [x] Handle expired or invalid OTP codes
- [x] Mark the user's email as verified after successful verification

**Endpoint:**

```http
POST /api/auth/verify
```

### 6. Create Resend OTP Endpoint

- [x] Create the resend OTP request DTO
- [x] Validate the email input using FluentValidation
- [x] Check whether the user exists
- [x] Check the user's email verification status
- [x] Generate a new OTP code securely
- [x] Send a new verification email
- [x] Replace or invalidate the previous OTP

**Endpoint:**

```http
POST /api/auth/resend
```

### 7. Create Registration Endpoint

- [x] Create the registration request DTO
- [x] Validate registration input using FluentValidation
- [x] Check whether the email already exists
- [x] Hash the user's password securely
- [x] Create the user
- [x] Assign the default role
- [x] Generate an OTP verification code
- [x] Send the OTP verification email
- [x] Return an appropriate API response
- [x] Handle validation and duplicate-user errors correctly

**Endpoint:**

```http
POST /api/auth/register
```

### 8. Create Login Endpoint

- [x] Create the login request DTO
- [x] Validate login credentials using FluentValidation
- [x] Find the user by email
- [x] Verify the password securely
- [x] Check whether the account is active
- [x] Check email verification status
- [x] Generate the authentication token
- [x] Generate a refresh token
- [x] Store the refresh token securely
- [x] Return the authentication response
- [x] Handle invalid credentials securely

**Endpoint:**

```http
POST /api/auth/login
```

### 9. Configure Auth Controller

- [x] Add the registration endpoint
- [x] Add the login endpoint
- [x] Add the resend OTP endpoint
- [x] Add the verify OTP endpoint
- [x] Delegate authentication logic to `IAuthService`
- [x] Return responses consistently using `HandleResult`

---

## 4. Implement Password Reset, Token Management, and User Profile

### 1. Create Password Reset OTP Functionality

- [x] Create the password reset email request DTO
- [x] Reuse the OTP verification DTO
- [x] Validate email and OTP input using FluentValidation
- [x] Generate a secure OTP for password reset requests
- [x] Store password reset OTP information securely
- [x] Configure OTP expiration
- [x] Track and limit OTP verification attempts
- [x] Create the password reset email template
- [x] Send the password reset OTP email
- [x] Keep password reset OTPs separate from email verification OTPs
- [x] Replace or invalidate the previous OTP when a new reset OTP is generated

**Endpoints:**

```http
POST /api/auth/send/reset
POST /api/auth/verify/reset
```

### 2. Create Password Reset Verification

- [x] Receive and validate the user's email and OTP
- [x] Find the user by email
- [x] Verify the submitted password reset OTP
- [x] Handle invalid or expired OTP codes
- [x] Handle maximum OTP verification attempts
- [x] Generate a temporary JWT after successful OTP verification
- [x] Include the required password reset permission in the JWT
- [x] Limit the temporary JWT to password reset functionality

**Endpoint:**

```http
POST /api/auth/verify/reset
```

### 3. Create Reset Password Endpoint

- [x] Create the reset password request DTO
- [x] Validate the new password using FluentValidation
- [x] Require authorization using the temporary password reset JWT
- [x] Require the `user.resetPassword` permission
- [x] Extract the authenticated user's email from JWT claims
- [x] Reset the user's password using ASP.NET Core Identity
- [x] Return an appropriate API response

**Endpoint:**

```http
POST /api/auth/reset
```

### 4. Create Refresh Token Functionality

- [x] Generate refresh tokens during authentication
- [x] Store refresh tokens securely
- [x] Configure refresh tokens using HTTP-only cookies
- [x] Read the refresh token from the request cookie
- [x] Validate the refresh token
- [x] Check refresh token expiration and revocation status
- [x] Generate a new access token
- [x] Implement refresh token rotation
- [x] Revoke or replace the previous refresh token
- [x] Update the refresh token cookie
- [x] Handle missing, invalid, expired, or revoked refresh tokens

**Endpoint:**

```http
GET /api/auth/refresh
```

### 5. Create Logout Functionality

- [x] Require authenticated access
- [x] Require the `user.read` permission
- [x] Read the refresh token from the request cookie
- [x] Revoke the refresh token
- [x] Clear the refresh token cookie
- [x] Prevent the revoked refresh token from being used again

**Endpoint:**

```http
GET /api/auth/logout
```

### 6. Create Get Current User Functionality

- [x] Require authenticated access
- [x] Require the `user.read` permission
- [x] Extract the user's email from JWT claims
- [x] Find the authenticated user
- [x] Return the current user's information
- [x] Ensure sensitive information is not exposed

**Endpoint:**

```http
GET /api/auth/me
```

---

## 🔐 Permission Architecture

SecureAuth uses a **Permission-Based Authorization** architecture built on top of ASP.NET Core Authorization.

Roles are responsible for grouping permissions, while permissions define the actual actions that a user is authorized to perform.

```text
User
 │
 ▼
Roles
 │
 ▼
Permissions
 │
 ▼
JWT Claims
 │
 ▼
Authorization Policy
 │
 ▼
Protected Endpoint
```

### Roles and Permissions

A user can have one or more roles.

Each role contains a collection of permissions.

For example:

```text
Admin
├── user.read
├── user.create
├── user.update
├── user.delete
└── user.resetPassword

User
├── user.read
└── user.resetPassword
```

During authentication, the application collects the permissions associated with the user's roles and adds them to the JWT as claims.

Example:

```text
JWT
│
├── email: user@example.com
├── role: User
├── permission: user.read
└── permission: user.resetPassword
```

### Permission Flow

The authorization flow works as follows:

1. The user sends an authenticated request.
2. ASP.NET Core validates the JWT.
3. The permission claims are loaded into the authenticated user's `ClaimsPrincipal`.
4. The `HasPermission` attribute identifies the required permission.
5. The custom `PermissionPolicyProvider` creates or resolves the authorization policy.
6. `PermissionAuthorizationHandler` checks whether the authenticated user has the required permission.
7. The request is authorized or rejected with `403 Forbidden`.

### Protecting Endpoints

Endpoints can be protected using the custom `HasPermission` attribute.

```csharp
[HasPermission("user.read")]
[HttpGet("me")]
public async Task<IActionResult> GetMe()
{
    var email = User.FindFirstValue(ClaimTypes.Email);

    var result = await _authService.GetMe(email!);

    return HandleResult(result);
}
```

The endpoint is executed only when the authenticated user has the required permission.

### Available Permissions

| Permission           | Description                                                                   |
| -------------------- | ----------------------------------------------------------------------------- |
| `user.read`          | Allows access to authenticated user information and protected user operations |
| `user.resetPassword` | Allows the user to reset their password after successful OTP verification     |

> **Note:** `user.resetPassword` is a temporary permission used by the password reset JWT generated after successful password reset OTP verification.

---

## 🔄 Authentication Flow

### Registration and Email Verification

```text
Register
   │
   ▼
Create User
   │
   ▼
Assign Default Role
   │
   ▼
Generate Email Verification OTP
   │
   ▼
Send Verification Email
   │
   ▼
Verify OTP
   │
   ▼
Email Confirmed
```

### Login and Refresh Token

```text
Login
   │
   ▼
Validate Credentials
   │
   ▼
Generate Access Token
   │
   ├──────────────► Return JWT
   │
   ▼
Generate Refresh Token
   │
   ▼
Store Refresh Token
   │
   ▼
Set HTTP-only Cookie
```

When the access token expires:

```text
Client
   │
   ▼
GET /api/auth/refresh
   │
   ▼
Validate Refresh Token Cookie
   │
   ▼
Generate New Access Token
   │
   ▼
Rotate Refresh Token
   │
   ▼
Return New Authentication Token
```

### Password Reset Flow

```text
Send Reset Request
   │
   ▼
Generate Reset OTP
   │
   ▼
Send OTP by Email
   │
   ▼
Verify Reset OTP
   │
   ▼
Generate Temporary Reset JWT
   │
   ▼
Temporary Permission:
user.resetPassword
   │
   ▼
POST /api/auth/reset
   │
   ▼
Reset Password
```

---

## 🌐 API Endpoints

### Authentication

| Method | Endpoint             | Description                   | Authorization |
| ------ | -------------------- | ----------------------------- | ------------- |
| `POST` | `/api/auth/register` | Register a new user           | No            |
| `POST` | `/api/auth/login`    | Authenticate a user           | No            |
| `POST` | `/api/auth/resend`   | Resend email verification OTP | No            |
| `POST` | `/api/auth/verify`   | Verify email OTP              | No            |

### Password Reset

| Method | Endpoint                 | Description                                                  | Authorization        |
| ------ | ------------------------ | ------------------------------------------------------------ | -------------------- |
| `POST` | `/api/auth/send/reset`   | Send password reset OTP                                      | No                   |
| `POST` | `/api/auth/verify/reset` | Verify password reset OTP and generate a temporary reset JWT | No                   |
| `POST` | `/api/auth/reset`        | Reset the user's password                                    | `user.resetPassword` |

### Token Management

| Method | Endpoint            | Description                                             | Authorization        |
| ------ | ------------------- | ------------------------------------------------------- | -------------------- |
| `GET`  | `/api/auth/refresh` | Refresh the access token using the refresh token cookie | Refresh Token Cookie |
| `GET`  | `/api/auth/logout`  | Revoke the refresh token and log out the user           | `user.read`          |

### Current User

| Method | Endpoint       | Description                              | Authorization |
| ------ | -------------- | ---------------------------------------- | ------------- |
| `GET`  | `/api/auth/me` | Get the authenticated user's information | `user.read`   |

---

## 🛠️ Technologies

- **.NET / ASP.NET Core**
- **ASP.NET Core Identity**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Refresh Tokens**
- **HTTP-only Cookies**
- **Permission-Based Authorization**
- **FluentValidation**
- **Otp.NET**
- **MimeKit**
- **MailKit**
- **Swagger / OpenAPI**
- **xUnit**
- **Git / GitHub**

---

## 🏗️ Project Architecture

SecureAuth follows a layered architecture that separates API concerns, application logic, domain models, and infrastructure implementations.

```text
SecureAuth
│
├── SecureAuth.Api
│   ├── Attributes
│   ├── Authorization
│   │   ├── PermissionAuthorizationHandler.cs
│   │   ├── PermissionPolicyProvider.cs
│   │   └── PermissionRequirement.cs
│   ├── Controllers
│   ├── Middlewares
│   ├── Program.cs
│   ├── appsettings.json
│   └── SecureAuth.Api.http
│
├── SecureAuth.Application
│   ├── Common
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
