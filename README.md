# RealTimeQuiz API

A real-time quiz and poll backend API built with .NET 9 and C#. Supports live result updates, anonymous and authenticated participants, and multiple question types.

---

## Architecture

Clean Architecture with 5 layers:

- **Domain** — Entities, enums
- **Application** — CQRS commands/queries, interfaces, DTOs, validators, specifications
- **Infrastructure** — JWT, password hashing, SignalR hub service, Redis cache service
- **Persistence** — EF Core DbContext, generic repository, specification evaluator, configurations
- **MainApi** — Controllers, SignalR hub, middleware, DI registration

---

## Patterns & Technologies

- **CQRS** with MediatR — commands and queries are separate, handled by dedicated handlers
- **Generic Repository + Specification Pattern** — flexible, reusable data access with `IGenericRepository<T>` and `BaseSpecification<T>`
- **SignalR** — real-time communication for live answer updates, participant counter, question navigation, and final results
- **Redis** — caching for quiz data and analytics with cache invalidation on data changes
- **JWT Authentication** — secure token-based auth for creators
- **FluentValidation** — request DTO validation
- **Rate Limiting** — IP-based rate limiting on all endpoints, stricter limits on auth and submission endpoints
- **BCrypt** — password hashing
- **Global Exception Handler** — centralized error handling middleware

---

## Tech Stack

- .NET 9 / C#
- PostgreSQL (via Docker)
- Redis (via Docker)
- Entity Framework Core + Npgsql
- MediatR
- SignalR
- FluentValidation
- BCrypt.Net

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/RealTimeQuizApi.git
cd RealTimeQuizApi
```

### 2. Start Docker containers

```bash
docker-compose up -d
```

This starts PostgreSQL on port `5432` and Redis on port `6379`.

### 3. Run migrations

```bash
dotnet ef migrations add InitialCreate --project RealTimeQuiz.Persistence --startup-project RealTimeQuizApi
dotnet ef database update --project RealTimeQuiz.Persistence --startup-project RealTimeQuizApi
```

### 4. Run the project

```bash
dotnet run --project RealTimeQuizApi
```

API will be available at `https://localhost:7149/scalar/v1`

---

## API Overview

| Area | Endpoints |
|------|-----------|
| Auth | Register, Login, Change Password, Get Current User |
| Quiz | Create, Update, Delete, Get, Get All, Get My Quizzes, Activate, Navigate, Announce Results |
| Questions | Create, Update, Delete, Get by Quiz |
| Answer Options | Create, Update, Delete, Get by Question |
| Responses | Start, Submit Single Answer, Submit All Answers, Complete, Get by Quiz |
| Analytics | Get Quiz Analytics |

---

## Real-time Events (SignalR)

Connect to `/hubs/quiz` and join a quiz room with `JoinQuiz(quizId)` to receive:

| Event | Description |
|-------|-------------|
| `ParticipantJoined` | New player joined the quiz |
| `AnswerSubmitted` | A player submitted an answer |
| `QuizCompleted` | A player completed the quiz |
| `QuestionNavigationSync` | Creator moved to next question |
| `FinalResultsAnnouncement` | Creator announced final results |

---

## Environment Configuration

`appsettings.json`:

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=realtimequiz_db;Username=postgres;Password=postgres"
  },
  "Redis": {
    "ConnectionString": "localhost:6379",
    "InstanceName": "QuizApi:"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "RealTimeQuiz",
    "Audience": "RealTimeQuizUsers",
    "ExpiryMinutes": 60
  }
}
```
