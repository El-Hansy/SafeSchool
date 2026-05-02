# 🏛️ System Architecture & Engineering Constitution

This document defines the technical principles, architecture, and best practices for building and maintaining the School NFC platform.

---

## 1. 🎯 Core Principles

1. Build a **modular, multi-tenant SaaS platform**, not a single-school system.
2. Favor **simplicity over premature complexity** (start modular monolith → evolve if needed).
3. Enforce **feature-based architecture** aligned with Feature Modules.
4. All features must be **tenant-aware and feature-flag controlled**.
5. Prioritize:
   - Low infrastructure cost 💰
   - Maintainability 🧩
   - Security 🔐
   - Observability 📊

---

## 2. 🧱 System Architecture

### Architecture Style
- Start with **Modular Monolith**
- Structure by **Feature Modules (Domain-driven)**
- Evolve to microservices only when scaling demands it

### High-Level Layers

- Presentation Layer (Web / Mobile)
- Application Layer (Use Cases / Services)
- Domain Layer (Business Logic)
- Infrastructure Layer (DB, External Services)

---

## 3. 🌐 Web Application

### Stack
- Next.js (latest)
- React
- TypeScript

### Standards

- Use **App Router (Next.js)**  
- Use **Server Components by default**
- Use **Client Components only when necessary**

### State Management
- Server state → React Query (TanStack Query)
- Local UI state → React hooks / Zustand (lightweight)

### UI
- Use a consistent design system (e.g. Tailwind)
- Build reusable components per module

### API Communication
- Use typed API clients (OpenAPI or custom TS types)
- Centralized API layer

---

## 4. ⚙️ Backend / API

### Stack
- .NET (latest)
- ASP.NET Core Web API

### Architecture

- Feature-based folder structure:
  - Identity
  - Transport
  - Wallet
  - Attendance
  - etc.

- Apply:
  - Clean Architecture (lightweight, not over-engineered)
  - CQRS (only where needed)

### Key Practices

- All endpoints must:
  - Be **tenant-aware**
  - Check **feature flags**

- Use:
  - Middleware for tenant resolution
  - Middleware for authorization

---

## 5. 🗄️ Database

### Stack
- PostgreSQL

### Design Principles

- Use **single database, multi-tenant (tenant_id column)**
- Every table must include:
  - `tenant_id`
  - `created_at`
  - `updated_at`

### Migrations
- Use EF Core migrations
- Version-controlled schema

### Performance
- Index:
  - tenant_id
  - foreign keys
  - frequently queried fields

---

## 6. 📱 Mobile Application

### Stack
- Flutter (Dart)
- Native layers:
  - Android → Kotlin / Gradle
  - iOS → Swift / CocoaPods

### Architecture

- Feature-based structure (same modules as backend)
- Use clean separation:
  - UI
  - State
  - Services

### Local Storage
- SQLite (sqflite)

Use for:
- Offline NFC scans
- Cached data
- Temporary queues

---

## 7. 📡 NFC & Offline Strategy

- NFC must work **offline-first**
- Store scans locally in SQLite
- Sync with backend when online

### Conflict Handling
- Use timestamps
- Use idempotent APIs

---

## 8. 🔐 Security

- JWT-based authentication
- Role-based authorization
- Feature-based authorization (critical)

### Rules

- Never trust frontend
- Always validate:
  - tenant access
  - feature availability
  - user role

---

## 9. 🧩 Feature Flag System

- Central Feature Flag service

### Rules

- Every module must check:
  - Is feature enabled for tenant?

- Enforced at:
  - API level
  - UI level

---

## 10. 📦 API Design

- RESTful APIs
- Versioned endpoints (`/api/v1/`)

### Standards

- Consistent response format
- Use DTOs (never expose DB models)
- Pagination for lists

---

## 11. 🚀 Deployment & Cost Optimization

### Goals
- Minimum cost
- Maximum scalability

### Recommended Setup

- Backend:
  - Dockerized .NET API
  - Deploy on:
    - Fly.io / Railway / Render (low cost)

- Database:
  - Managed PostgreSQL (Neon / Supabase / Railway)

- Web:
  - Vercel (ideal for Next.js)

- Storage:
  - Use S3-compatible storage (low-cost providers)

---

## 12. 📊 Observability

- Logging:
  - Structured logs (Serilog)

- Monitoring:
  - Basic metrics (CPU, memory, API latency)

- Error tracking:
  - Centralized logging system

---

## 13. 🧪 Testing Strategy

- Unit tests for business logic
- Integration tests for APIs
- Minimal UI testing (focus on core flows)

---

## 14. 📁 Code Organization Rules

- Organize by **feature/module**, not by technical layer
- Avoid “God services”
- Keep functions small and testable

---

## 15. 🚧 Evolution Strategy

1. Start:
   - Modular Monolith
   - Single database

2. Scale:
   - Extract heavy modules (e.g., transport, wallet)

3. Advanced:
   - Event-driven architecture (Kafka / queues if needed)

---

## 16. ⚠️ Non-Negotiable Rules

- Every feature must be:
  - Tenant-aware
  - Feature-flagged

- No direct DB access from UI
- No business logic in controllers
- No feature without module mapping

---

## 🧭 Final Principle

> Build it simple, modular, and controlled.
> Scale only when reality demands it — not before.