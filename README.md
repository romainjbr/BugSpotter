[![Project Board](https://img.shields.io/badge/Project%20Board-Kanban-blue?style=for-the-badge)](https://github.com/users/romainjbr/projects/3)

# BugSpotter: a Bug Tracking Tool (for actual, living bugs!)

A clean architecture ASP.NET Core application showcasing workflow, maintainability and testing. 

## ▶ Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Authentication & Authorization](#authentication)
- [Development Workflow](#development-workflow)
- [CI with GitHub Actions](#ci)
- [Tech Stack](#tech-stack)
- [Run Locally](#run-locally)

## ▶ Overview  <a name="overview"></a>

**BugSpotter** is a small and production-style application built to demonstrate how I approach software development using:

- **Clean Architecture** (Presentation / Core / Infrastructure layers)
- **ASP.NET Core Minimal API** 
- **Entity Framework Core**
- **Unit Testing with xUnit & Moq** 
- **Continuous Integration**
- **Kanban Board (Github Project) + Pull Request–based workflow**

The app models a simple bug tracking system where users can create and manage Bugs, assign Species and work through CRUD operations. 
This project also displays how I maintain a clean workflow from idea > ticket > implementation > PR.

## ▶ Architecture <a name="architecture"></a>

Clean Architecture was chosen to structure the project because it:
- keeps business rules independent from frameworks
- improves testability, maintanability and scalability

<details> 
  <summary><strong> $${\color{blue}Check \space the \space Project \space Structure \space Tree:}$$ </strong></summary>
<pre>
├── src
│   ├── Core
│   │   ├── Core.csproj
│   │   ├── Dtos
│   │   │   ├── Bugs
│   │   │   │   ├── BugCreateDto.cs
│   │   │   │   ├── BugMapper.cs
│   │   │   │   ├── BugReadDto.cs
│   │   │   │   └── BugUpdateDto.cs
│   │   │   ├── Sightings
│   │   │   │   ├── SightingCreateDto.cs
│   │   │   │   ├── SightingMapper.cs
│   │   │   │   ├── SightingReadDto.cs
│   │   │   │   └── SightingUpdateDto.cs
│   │   │   └── Users
│   │   │       ├── UserCreateDto.cs
│   │   │       ├── UserLoginDto.cs
│   │   │       ├── UserMapper.cs
│   │   │       └── UserReadDto.cs
│   │   ├── Entities
│   │   │   ├── Bug.cs
│   │   │   ├── Sighting.cs
│   │   │   └── User.cs
│   │   ├── Interfaces
│   │   │   ├── Repositories
│   │   │   │   ├── IRepository.cs
│   │   │   │   └── IUserRepository.cs
│   │   │   └── Services
│   │   │       ├── IBugService.cs
│   │   │       ├── IPasswordHasherService.cs
│   │   │       ├── ISightingService.cs
│   │   │       ├── ITokenService.cs
│   │   │       └── IUserService.cs
│   │   └── Services
│   │       ├── BugService.cs
│   │       ├── SightingService.cs
│   │       └── UserService.cs
│   ├── Infrastructure
│   │   ├── Data
│   │   │   └── BugDb.cs
│   │   ├── Infrastructure.csproj
│   │   ├── Migrations
│   │   │   ├── 20251119101608_InitialCreate.Designer.cs
│   │   │   ├── 20251119101608_InitialCreate.cs
│   │   │   ├── 20251119104529_AddModelsRelations.Designer.cs
│   │   │   ├── 20251119104529_AddModelsRelations.cs
│   │   │   └── BugDbModelSnapshot.cs
│   │   ├── Repositories
│   │   │   ├── EfRepository.cs
│   │   │   └── EfUserRepository.cs
│   │   ├── Services
│   │   │   ├── PasswordHasherService.cs
│   │   │   └── TokenService.cs
│   │   └── Settings
│   │       └── JwtOptions.cs
│   └── Presentation
│       ├── Endpoints
│       │   ├── BugEndpoints.cs
│       │   ├── SightingEndpoints.cs
│       │   └── UserEndpoints.cs
│       ├── Extensions
│       │   ├── AuthExtension.cs
│       │   └── SwaggerAuthConfigExtension.cs
│       ├── Presentation.csproj
│       └── Program.cs
└── test
    ├── Core.Tests
    │   ├── Core.Tests.csproj
    │   └── Services
    │       ├── BugServiceTest.cs
    │       ├── SightingServiceTest.cs
    │       └── UserServiceTest.cs
    ├── Infrastructure.Tests
    │   ├── EfRepositoryTest.cs
    │   ├── Infrastructure.Tests.csproj
    │   └── Services
    │       ├── PasswordHasherServiceTest.cs
    │       └── TokenServiceTest.cs
    └── Presentation.Tests
        ├── AuthenticationHelper
        │   └── TestAuthHandler.cs
        ├── EndpointsTest
        │   ├── BugEndpointsTests.cs
        │   ├── SightingEndpointsTests.cs
        │   └── UserEndpointsTests.cs
        └── Presentation.Tests.csproj
</pre>
</details>

## ▶ Authentication & Authorization <a name="authentication"></a>

The project includes a developed authentication system using **JWT (JSON Web Tokens)**

- Users can register and log in.
- Passwords are securely hashed.
- Access tokens are issued on successful login/registration.
- Protected endpoints require a valid JWT.
- Basic role-based authorization is implemented (e.g., 'Admin', 'User').
- Certain API routes are restricted to admins only.

## ▶ Development Workflow <a name="development-workflow"></a>

This repository uses a GitHub Project board to organise work:
- Each task is represented as a ticket (issue)
- Every feature/fix is developed on a separate branch
- Each task involves a Pull Request referencing the issue
- PRs requires the pipeline to pass before merge

[Check the board here!](https://github.com/users/romainjbr/projects/3)

## ▶ CI with GitHub Actions <a name="ci"></a>

A ci.yml workflow automatically runs on every:
- Push
- Pull Request

The pipeline performs:
- Restore
- Build
- Run unit tests

This ensures consistent quality and prevents regressions.

## ▶ Tech Stack <a name="tech-stack"></a>
- **Backend:** ASP.NET Core (Minimal APIs), C#  
- **Architecture:** Clean Architecture (Core / Infrastructure / Presentation)  
- **Database:** Entity Framework Core, SQLite  
- **Testing:** xUnit, Moq  
- **CI:** GitHub Actions  
- **Project Management:** GitHub Projects (Kanban Board), Pull Request workflow  

## ▶ Run Locally <a name="run-locally"></a>

1. Clone the project

```bash
  git clone https://github.com/yourname/BugSpotter.git
  cd BugSpotter
```

2. Apply Migrations 

```bash
  dotnet ef database update --project src/Infrastructure
```

3. Run the API

```bash
  dotnet run --project src/Presentation/Presentation.csproj
```

4. Access Swagger

```bash
  https://localhost:[THE_PORT_APPEARING_IN_CLI]/swagger/index.html
```

  
