# Our Library  Backend

The backend for the Our Library app: a **.NET 10** minimal Web API secured with **Auth0**,
backed by a **SQL Server** database project.

> Part of the **[Our Library](https://github.com/users/ayindig1948/projects/1)** project.
> Frontend SPA: **[Our-Library-frontEnd](https://github.com/ayindig1948/Our-Library-frontEnd)** (Vite + React).

## Tech stack

- **.NET 10** — ASP.NET Core Minimal APIs
- **Auth0** — JWT bearer authentication (validates signature, audience, expiry)
- **SQL Server** — SSDT database project (tables + stored procedures)
- **Serilog** — structured logging (Console, File, Seq sinks)
- Output caching, rate limiting, and CORS

## Solution layout (`LibraryDemoApp.slnx`)

| Project | Description |
| --- | --- |
| `TheLibraryApi` | Minimal API endpoints — Auth0 JWT auth, CORS, caching, rate limiting, Serilog logging |
| `LibraryTools` | Data-access library and domain models |
| `LibraryTools.Tests` | Unit tests for `LibraryTools` |
| `LibraryDb` | SQL Server Database Project — tables and stored procedures |

## Configuration & secrets

Secrets are **not** committed. They come from .NET **user-secrets** (and/or environment
variables in production). `appsettings.json` ships with empty placeholders:

```jsonc
"ConnectionStrings": { "LibraryDb": "" },
"Auth0": { "Domain": "", "Audience": "" }
```

Set them locally with:

```bash
cd TheLibraryApi
dotnet user-secrets set "ConnectionStrings:LibraryDb" "<your connection string>"
dotnet user-secrets set "Auth0:Domain"   "<your-tenant>.us.auth0.com"
dotnet user-secrets set "Auth0:Audience" "<your-api-audience>"
```

## Running

```bash
dotnet restore
dotnet run --project TheLibraryApi
```

The API validates the Auth0 JWT (signature, audience, expiry) on protected endpoints, so
the frontend must send a valid `Authorization: Bearer <token>`.

## Database

The schema (tables + stored procedures) lives in `LibraryDb/dbo`. Build/publish the
`LibraryDb` SQL project to deploy it to your SQL Server instance.
