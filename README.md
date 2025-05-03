# 📦 Task Management Service API

This project provides a .NET8 REST API for managing tasks with a simple lifecycle. It's inspired of **SOLID**, **DDD** and **Clean Architecture**, integrates with **RabbitMQ** for event publishing, and **MySQL** for persistence.

### ✅ Features

- Task lifecycle management (NotStarted → InProgress → Completed) via REST API
- Domain-Driven Design (Entities, Services, Repositories, Anti-Corruption Layer, Domain Events...)
- FluentValidation for input validation
- Error handling with corresponding response statuses
- Asynchronous approach and messaging with RabbitMQ
- MySQL via Entity Framework Core
- Docker and docker-compose support

### 🔜 Future Enhancements

- Add authentication/authorization
- UI for task management
- Retry and dead-letter queues for RabbitMQ
- Add a transaction or lock to update the status
- Tracking task or status transitions
- Observability(OpenTelemetry): metrics, tracing, structured logging
- Add sorting/filtering for the GET '/tasks' endpoint
- Improve the health check to also check dependencies
- Cover all elements with unit tests
- Place all confidential variables(logins, passwords, connection strings) in secrets

### 📬 Messaging
The service publishes domain event TaskCompletedEvent to RabbitMQ when the status changes from InProgress to Completed.

This event is sent to the RabbitMQ fanout exchange. The consumer runs in the API project as a background worker, potentially can be moved to a separate one, as it is located in a separate TaskManagementSystem.EventHandler project.

The consumer adds an information log "TaskCompleted message has been handled. TaskId: {taskId}" if the message was processed successfully, or an error log if an error occurred.

### 📁 Project Structure

```bash
TaskManagementSystem/
├── src/
│   ├── TaskManagementSystem.Api            # ASP.NET Core Web API entry point
│   ├── TaskManagementSystem.Application    # UseCases/Services, Interfaces, DTOs
│   ├── TaskManagementSystem.Domain         # Entities, Enums, Domain Events
│   ├── TaskManagementSystem.EventHandler   # Event consumer
│   ├── TaskManagementSystem.Infrastructure # RabbitMQ/Event publisher, DB implementations
│   ├── TaskManagementSystem.Tests          # unit tests  
├── build/
│   ├── docker-compose.yml                  # API + SQL + RabbitMQ setup
└── README.md
```
### 🟡 Trade-offs
- MySql database with volume storage is used to simplify development — not suitable for production
- Simple logging instead of a full observability infrastructure

### 🔴 Limitations
- Authorization is not supported yet
- No transactional approach, possible mismatches with simultaneous task updates
- No extensive error handling of RabbitMQ events


# 🚀 Getting Started

### 🧰 Prerequisites
- .NET 8 SDK
- Docker

### 🟢 Run the service locally

1. Start API + RabbitMQ + MySQL via Docker:

```
docker-compose -f build/docker-compose.yml up -d
```

2. Access endpoints:

- Swagger UI:   http://localhost:5197
- RabbitMQ UI:  http://localhost:15672
- MySQL:        localhost:3306

3. Optionally, when the RabbitMQ and MySQL containers are running, the API can be exposed via an IDE (Visual Studio, Rider) with the TaskManagementSystem.Api project and the "http" launch profile, the API endpoint in this case is http://localhost:5196.

🔧 Configuration
Configurations are managed via appsettings.json and/or environment variables in docker-compose.
```
{
  "ConnectionStrings": {
    "TaskManagementSystemDb": ""
  },
  "RabbitMqSettings": {
    "TaskExchange": "",
    "TaskQueue": "",
    "Host": "",
    "Username": "",
    "Password": ""
  }
}
```