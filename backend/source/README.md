# Task Management System - Backend

## Overview
This backend API provides a task management system with authentication, user management, and task tracking. It is built with **.NET 6** and **Entity Framework Core**.  

Key features include:  
- JWT-based authentication and role-based authorization (`Admin`, `User`)  
- CRUD operations for tasks  
- Task assignment to users and task status tracking  
- Unit and integration tests to ensure reliability  

---

## 🛠️ Tech Stack
- **Framework**: .NET 6 (ASP.NET Core Web API)  
- **Database**: PostgreSQL (or InMemory for dev/testing)  
- **ORM**: Entity Framework Core  
- **Authentication**: ASP.NET Identity + JWT  
- **Validation**: Data Annotations  
- **Exception Handling**: Global error handler with HTTP status codes  
- **Testing**: xUnit / NUnit with Moq  

---

## 🔑 Authentication & Authorization
- JWT-based authentication  
- User registration and login endpoints  
- Role-based access: `Admin` and `User`  
- Secure password hashing using Identity  

### Endpoints
- `POST /api/auth/register` – Register a new user  
- `POST /api/auth/login` – Authenticate a user  

---

## 📋 Task Management API
- CRUD operations for tasks  
- Filter tasks by status or assignee  
- Assign tasks to users  
- Status transitions: `TODO → IN_PROGRESS → DONE`  

### Endpoints
- `GET /api/tasks?status=&assignee=` – Get tasks with optional filters  
- `POST /api/tasks` – Create a new task  
- `PUT /api/tasks/{id}` – Update a task  
- `DELETE /api/tasks/{id}` – Delete a task  
- `GET /api/users` – Get list of users (for assignment)  

---

## 🧪 Testing

### 1. Service Layer Unit Tests
- Uses **xUnit** and **Moq** for mocking repositories  
- Covers:
  - Task creation, update, deletion  
  - Task assignment to users  
  - User registration and role assignment  

