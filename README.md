# GuideX — QR Code Management API

[![CI](https://github.com/Mohammedkhaled96/GuideX2025/actions/workflows/ci.yml/badge.svg)](https://github.com/Mohammedkhaled96/GuideX2025/actions/workflows/ci.yml)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-8-512BD4)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=white)
![Swagger](https://img.shields.io/badge/OpenAPI-Swagger-85EA2D?logo=swagger&logoColor=black)

A REST API for managing QR codes attached to items and tracking every scan, built with ASP.NET Core 8 and Entity Framework Core on MySQL.

## ✨ Features

- **QR code catalog** — QR codes linked to items and owned by users
- **Scan tracking** — every scan is stored for analytics
- **Paginated queries** — fetch a user's QR codes page by page or all at once
- **Service layer** — controllers depend on `IQRCodeService`, keeping business logic testable
- **OpenAPI / Swagger** — interactive documentation out of the box

## 🏗️ Project structure

```
guidex/
├── QRCodeManagement.sln
└── QRCodeManagement.API/
    ├── Controllers/        # HTTP endpoints (QRCodeController)
    ├── Services/           # Business logic + interfaces
    ├── Data/               # ApplicationDbContext (EF Core)
    ├── Models/             # Item, QRCode, Scan, User
    ├── DTOs/               # Response contracts
    └── migration_script.sql
```

## 🔌 API

| Method | Route | Description |
|---|---|---|
| `GET` | `/api/QRCode/user/{userId}/paginated` | QR codes of a user, paginated |
| `GET` | `/api/QRCode/user/{userId}/all` | All QR codes of a user |

## 🚀 Getting started

**Prerequisites:** .NET 8 SDK and a MySQL server.

1. Create the database with `guidex/QRCodeManagement.API/migration_script.sql`.
2. Set your connection string — never commit real credentials. Use user secrets:
   ```bash
   cd guidex/QRCodeManagement.API
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=127.0.0.1;database=guidex;user=root;password=YOUR_PASSWORD"
   ```
3. Run the API:
   ```bash
   dotnet run --project guidex/QRCodeManagement.API
   ```
4. Open Swagger at `https://localhost:<port>/swagger`.

## ⚙️ CI/CD

GitHub Actions restores, builds and runs tests on every push and pull request.
