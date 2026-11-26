<div align="center">

# 🎮 Games Catalog API

**A layered ASP.NET Core Web API for managing a catalog of games, platforms, emulators, categories, tags, and editorial content, with JWT authentication, Redis caching, and S3-compatible file storage (Cloudflare R2).**

![.NET](https://img.shields.io/badge/.NET-11-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4)
![Redis](https://img.shields.io/badge/Cache-Redis-DC382D?logo=redis&logoColor=white)
![JWT](https://img.shields.io/badge/Auth-JWT-000000?logo=jsonwebtokens&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver&logoColor=white)
![Cloudflare](https://img.shields.io/badge/Storage-Cloudflare%20R2-F38020?logo=cloudflare&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

</div>

---

## 📖 Overview

**Games Catalog API** is the backend of a game catalog website. It exposes a REST API that lets a front-end application browse and search games, platforms, emulators, and articles, while an authenticated admin area manages the whole catalog.

What it handles:

- 🕹️ **Games**: listing, search, filtering by platform / category / initial letter, popularity ranking
- 🧰 **Emulators**: catalog of emulators per platform
- 🖥️ **Platforms, categories and tags**: taxonomy used to organize everything
- 📰 **Posts, magazines and publicity**: editorial and advertising content
- ☁️ **File storage**: presigned and direct uploads of images and large media files to Cloudflare R2
- 🔐 **Authentication**: JWT login for the admin area

> [!IMPORTANT]
> **Scope.** This API manages catalog **metadata** only (names, descriptions, slugs, taxonomy, image references). It does not include game files or endpoints for distributing them. If you use it to store images or texts, make sure you have the right to publish them.
>
> **About this repository.** This project runs in production in a different (private) version. The code and the Docker files published here (`Dockerfile`, `docker-compose.yml`, and `.env.example`) exist so that **anyone can run this API on any machine** for development, study, and testing: one command brings up the API, Microsoft SQL Server, and Redis, with an initial admin user already created. Production-only settings (Cloudflare credentials, real domains, secrets, and infrastructure details) are intentionally **not** included.

## ✨ Features

- 🧱 **Layered architecture**: Domain, Application, Infrastructure, CrossCutting, and API projects
- 🔐 **JWT authentication** with ASP.NET Core Identity and role claims
- ⚡ **Distributed caching** (`IDistributedCache` / Redis) on public read endpoints
- 📄 **Consistent pagination** using `skip` / `take` on every list endpoint
- 🔎 **Search and filters**: by name, platform, category, initial letter, and popularity
- 📈 **Popularity counters** for games, platforms, and tags
- 🗂️ **Cloud storage integration** with presigned upload URLs
- 📦 **Large uploads**: multipart uploads up to **5 GB** on authenticated endpoints, with request size limits lifted, or direct-to-bucket uploads through presigned URLs
- 🛡️ **Public / admin separation**: public routes are open; management routes live under `adm/` or require a token
- 🗺️ **Dynamic XML sitemaps** generated straight from the database (SEO-ready, paginated at 45,000 URLs per file)
- 🔄 **Automatic migrations and seed**: the database is created, migrated, and seeded with an admin user on startup
- 🐳 **Docker ready**: `Dockerfile` + `docker-compose` bring up the API, SQL Server, and Redis with one command

## 🏗️ Architecture

The solution has **5 projects**, following a Clean Architecture style where dependencies point inward.

```
DownloadApi (solution)
│
├── Download.Domain            → Entities and domain interfaces (no dependencies)
│   ├── Entities/              EmulatorsModel, GameCategoryModel, GameModel, GameTagModel,
│   │                          MagazineModel, MisturebaModel, PlataformModels, PostModel,
│   │                          Publicity, User, Entity
│   └── Interfaces/
│
├── Download.Application       → Business logic
│   ├── DTOs/                  Data transfer objects
│   ├── Interfaces/            Service contracts
│   ├── Mappings/              DomainToDTOProfile (entity ↔ DTO mapping)
│   └── Services/              Emulator, GameCategory, GameModel, GameTags, Magazine,
│                              Mistureba, Plataform, PostModel, Publicity and Cloudflare services
│
├── Download.Infrastructure    → Data access and identity
│   ├── Context/               DownloadsDBContext (EF Core)
│   ├── Identity/              Login, Register, UserRole models and SeedData (initial user)
│   ├── Migrations/            EF Core migrations
│   └── Repositories/          One repository per aggregate
│
├── Download.CrossCutting      → Dependency injection
│   └── IoC/                   DependencyInjectionAPI, DependencyInjectionAPIServices
│
└── DownloadApi                → ASP.NET Core Web API (entry point)
    ├── Controllers/           REST controllers
    ├── appsettings.json
    └── Program.cs             Startup, pipeline, sitemap endpoints, migrate + seed
```

### Request flow

```
Client ──▶ Controller ──▶ Redis cache ──hit──▶ cached JSON
                │              │
                │            miss
                ▼              ▼
            Service ──▶ Repository ──▶ SQL Server (EF Core)
                │
                └──▶ Cloudflare R2 (image storage)
```

## 🔄 How the Application Works

`Program.cs` wires everything together. On startup the API does the following, in order:

1. **Configures upload limits**: multipart body up to **5 GB**, with the Kestrel / IIS request size limits removed, so large files can be uploaded through `/api/Storage/upload`.
2. **Configures CORS** from the `ALLOWED_ORIGINS` environment variable (origins separated by `;`).
3. **Connects to Redis** through `ConnectionStrings:RedisConnection` and registers it as `IDistributedCache`.
4. **Registers controllers, Swagger, and the services** of every layer via `AddDependencyInjectionAPI` and `AddDependencyInjectionAPIServices` (CrossCutting project).
5. **Configures ASP.NET Core Identity** (EF Core store) with a relaxed password policy: minimum 6 characters, no digit / uppercase / lowercase / symbol required.
6. **Configures JWT Bearer authentication**. The token is validated for issuer, lifetime, and signing key (`ClockSkew` is zero). The audience is not validated. `Jwt:Key` **must have at least 32 characters** or the app refuses to start.
7. **Builds the pipeline**: Swagger UI (Development only) → routing → CORS → authentication → authorization → controllers → sitemap endpoints.
8. **Migrates and seeds the database**: it runs `Database.MigrateAsync()` and then `SeedData.SeedAsync()`. If the database is not ready yet (common when SQL Server is still booting in Docker), it **retries up to 10 times, waiting 8 seconds between attempts**.

## 🧱 Tech Stack

| Area            | Technology                                       |
| --------------- | ------------------------------------------------ |
| Framework       | ASP.NET Core Web API (.NET 11)                   |
| Database        | Microsoft SQL Server 2022                        |
| ORM             | Entity Framework Core (code-first, migrations)   |
| Authentication  | ASP.NET Core Identity + JWT Bearer               |
| Caching         | `IDistributedCache` (Redis 7)                    |
| File storage    | Cloudflare R2 (S3-compatible, AWS SDK for .NET)  |
| Mapping         | DTO mapping profile (`DomainToDTOProfile`)       |
| Documentation   | Swagger / OpenAPI (Swashbuckle)                  |
| Containers      | Docker + Docker Compose                          |

## 🚀 Getting Started

### Option 1: Docker (recommended)

You need [Docker](https://docs.docker.com/get-docker/) with Docker Compose v2. The repository root ships with:

| File                 | Purpose                                                                          |
| -------------------- | -------------------------------------------------------------------------------- |
| `Dockerfile`         | Multi-stage build of the API (.NET 11 SDK → ASP.NET 11 runtime)                  |
| `docker-compose.yml` | Starts the API, **Microsoft SQL Server 2022**, and **Redis 7** on a shared network |
| `.env.example`       | Template of the environment variables used by the compose file                   |

#### Quick start

```bash
# 1. Clone the repository
git clone https://github.com/<your-user>/<your-repo>.git
cd <your-repo>

# 2. Create your .env from the template
cp .env.example .env          # PowerShell: Copy-Item .env.example .env

# 3. Edit .env and set YOUR OWN secrets (see the next section, this step is required)

# 4. Build and start everything
docker compose up -d --build

# 5. Follow the API logs (migrations + seed run here)
docker compose logs -f api
```

#### ⚠️ Step 3 is required: set your own secrets

The values in `.env.example` are **placeholders**. The API will not start with them, and you must never reuse any password you have seen in a public repository. Open `.env` and set:

| Variable         | What to put there                                                                                      |
| ---------------- | ------------------------------------------------------------------------------------------------------ |
| `JWT_KEY`        | A random secret with **at least 32 characters**. It signs every login token.                           |
| `DB_PASSWORD`    | SQL Server `SA` password. It must meet SQL Server's rules: 8+ characters with at least 3 of: upper case, lower case, digits, symbols. |
| `REDIS_PASSWORD` | Any strong password for Redis.                                                                         |

Generate strong values (pick the command for your system):

```bash
# Linux / macOS / WSL / Git Bash
openssl rand -base64 48        # use for JWT_KEY (64 characters)
openssl rand -hex 24           # use for REDIS_PASSWORD
```

```powershell
# PowerShell 7+ (JWT_KEY)
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
```

Then edit `.env`:

```env
JWT_KEY='paste-your-generated-key-here'
DB_PASSWORD='YourStr0ng!Passw0rd'
REDIS_PASSWORD='paste-your-redis-password-here'
```

Tips:

- **Wrap values in single quotes.** Docker Compose treats `$name` as a variable, so a key containing `$` would be silently altered.
- **For `DB_PASSWORD`, avoid `;`, `$`, quotes and spaces.** The password is also used inside a connection string.
- If `JWT_KEY` has fewer than 32 characters, the API stops at startup with `A Jwt:Key deve ter pelo menos 32 caracteres.` (the key must be at least 32 characters).
- The compose file refuses to run if `JWT_KEY`, `DB_PASSWORD` or `REDIS_PASSWORD` is empty.
- Changing `DB_PASSWORD` after the first run does not change the password stored in the existing SQL Server volume. Run `docker compose down -v` (this **deletes the data**) and start again.

```
┌───────────────────────── docker compose (network: api_net) ─────────────────────────┐
│                                                                                     │
│   ┌─────────────┐   depends_on (healthy)   ┌────────────────────┐                   │
│   │  aspnet_api │ ───────────────────────▶ │ sqlserver (2022)   │  volume: sql_data │
│   │  :7023→80   │                          │ :1433              │                   │
│   │             │   depends_on (started)   ├────────────────────┤                   │
│   │             │ ───────────────────────▶ │ redis_cache (7.0)  │ volume: redis_data│
│   └─────────────┘                          │ :6379 (password)   │                   │
│                                            └────────────────────┘                   │
└─────────────────────────────────────────────────────────────────────────────────────┘
```

What happens after `docker compose up`:

1. **SQL Server** starts and its health check (`SELECT 1` through `sqlcmd`) must pass before the API container starts (`start_period` of 40 s, up to 5 retries).
2. **Redis** starts with your password and a 1 GB memory limit.
3. The **API** starts, creates the database, applies all migrations, and runs the seed (see [Initial User (Seed)](#-initial-user-seed)).
4. When you see `Banco de dados atualizado e Seed executado com sucesso.` in the logs, everything is ready.

Check that it works:

```bash
curl http://localhost:7023/api/PlataformModels/0/10      # returns an empty list on a fresh database
```

| Service              | Container      | URL / Port                                     |
| -------------------- | -------------- | ---------------------------------------------- |
| API                  | `aspnet_api`   | `http://localhost:7023`                        |
| Swagger UI           | `aspnet_api`   | `http://localhost:7023/swagger` (Development)  |
| Microsoft SQL Server | `sqlserver`    | `127.0.0.1,1433` (user `SA`), host only        |
| Redis                | `redis_cache`  | `127.0.0.1:6379` (password required), host only|

> SQL Server and Redis are bound to `127.0.0.1`, so they are reachable from the machine running Docker but not from your network.

Useful commands:

```bash
docker compose ps                 # status of the containers
docker compose logs -f api        # API logs
docker compose down               # stop (data is kept in the volumes)
docker compose down -v            # stop and DELETE the database and Redis volumes
```

#### Troubleshooting

| Symptom                                                         | Likely cause and fix                                                                         |
| --------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| API exits with `A Jwt:Key deve ter pelo menos 32 caracteres.`   | `JWT_KEY` is too short, empty, or was altered by a `$`. Use single quotes and 32+ characters. |
| `required variable ... is missing a value`                      | Fill in `JWT_KEY`, `DB_PASSWORD`, and `REDIS_PASSWORD` in `.env`.                            |
| API cannot log in to SQL Server                                 | `DB_PASSWORD` was changed after the first run. Reset with `docker compose down -v`.          |
| SQL Server container is unhealthy                               | Weak password (it fails SQL Server's rules) or not enough memory (SQL Server needs about 2 GB). |
| SQL Server does not start on Apple Silicon (M1/M2/M3)           | The image is for amd64. Enable Rosetta / x86 emulation in Docker Desktop.                    |
| `/api/Storage` endpoints fail                                   | R2 is not configured. Add your `Cloudflare__*` settings to the `api` service.                |
| Port 7023 is already in use                                     | Change the left side of `"7023:80"` in `docker-compose.yml`.                                 |

### Option 2: Run locally

**Prerequisites**

- .NET 11 SDK
- A Microsoft SQL Server instance
- A Redis instance
- A Cloudflare R2 (or compatible) bucket, only if you need file storage

```bash
git clone https://github.com/<your-user>/<your-repo>.git
cd <your-repo>

dotnet restore

# Store secrets outside the repository
cd DownloadApi
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<sql-server-connection-string>"
dotnet user-secrets set "ConnectionStrings:RedisConnection" "localhost:6379"
dotnet user-secrets set "Jwt:Key" "<at-least-32-random-characters>"
dotnet user-secrets set "Jwt:Issuer" "<issuer>"
dotnet user-secrets set "Jwt:Audience" "<audience>"
dotnet user-secrets set "Jwt:ExpiryMinutes" "60"

# CORS origins are read from an environment variable (separated by ;)
export ALLOWED_ORIGINS="http://localhost:5021"   # PowerShell: $env:ALLOWED_ORIGINS="http://localhost:5021"

# Run (migrations and seed are applied automatically on startup)
dotnet run
```

> You do **not** need to run `dotnet ef database update`. The API applies pending migrations by itself when it starts.

## ⚙️ Environment Variables

### The `.env` file (Docker Compose)

`.env.example` contains only the variables the compose file actually reads:

```env
# SQL Server "SA" password (8+ chars, mixed types; avoid ; $ quotes and spaces)
DB_PASSWORD=

# JWT signing key: at least 32 random characters
JWT_KEY=

# Redis
REDIS_HOST=redis
REDIS_PORT=6379
REDIS_PASSWORD=

# CORS: allowed origins, separated by ;
ALLOWED_ORIGINS=http://localhost:5021;https://localhost:7023
```

| Variable          | Required | Description                                                                 |
| ----------------- | :------: | --------------------------------------------------------------------------- |
| `DB_PASSWORD`     | ✅       | SQL Server `SA` password (must meet SQL Server's complexity rules)          |
| `JWT_KEY`         | ✅       | Token signing key, **minimum 32 characters**. Generate your own.            |
| `REDIS_HOST`      | ✅       | Redis host (`redis` is the service name on the Docker network)              |
| `REDIS_PORT`      | ✅       | Redis port                                                                  |
| `REDIS_PASSWORD`  | ✅       | Redis password (also passed to `--requirepass`)                             |
| `ALLOWED_ORIGINS` | ✅       | CORS origins separated by `;` (each one must be a valid URL)                |

> 🔒 **Never commit your `.env` file.** It is listed in `.gitignore`; only `.env.example` is versioned.

### Application settings

ASP.NET Core maps double underscores (`__`) in environment variables to the `:` separator of `appsettings.json`. These are the keys the API reads:

| Setting                                | Required | Description                                                                 |
| -------------------------------------- | :------: | --------------------------------------------------------------------------- |
| `ConnectionStrings__DefaultConnection` | ✅       | SQL Server connection string                                                |
| `ConnectionStrings__RedisConnection`   | ✅       | Redis connection string (e.g. `redis:6379,password=...,abortConnect=false`) |
| `Jwt__Key`                             | ✅       | Signing key, **minimum 32 characters** (the app refuses to start otherwise) |
| `Jwt__Issuer`                          | ✅       | Token issuer (validated on every request)                                   |
| `Jwt__Audience`                        | ✅       | Audience written into the token on login (not validated by the API)         |
| `Jwt__ExpiryMinutes`                   | ✅       | Token lifetime in minutes                                                   |
| `Cloudflare__AccountUrl`               | Storage  | R2 endpoint, e.g. `https://<account-id>.r2.cloudflarestorage.com`           |
| `Cloudflare__BucketName`               | Storage  | R2 bucket name                                                              |
| `Cloudflare__AccessKeyId`              | Storage  | R2 access key id                                                            |
| `Cloudflare__SecretAccessKey`          | Storage  | R2 secret access key                                                        |
| `ALLOWED_ORIGINS`                      | ✅       | CORS origins separated by `;`                                               |
| `ASPNETCORE_ENVIRONMENT`               | ❌       | `Development` enables Swagger UI. Use `Production` when you deploy.         |
| `ASPNETCORE_URLS`                      | ❌       | Listening address (`http://+:80` inside the container)                      |

> ☁️ **File storage:** the Cloudflare section is empty in this public version. The catalog endpoints work without it, but the `/api/Storage` endpoints need your own `Cloudflare__*` settings added to the `api` service.

## 👤 Initial User (Seed)

On startup, right after the migrations, the API runs `SeedData.SeedAsync()` (in `Download.Infrastructure/Identity`). If the `Users` table is **empty**, it creates one initial user so you can log in and manage the catalog.

| Field    | Default value          |
| -------- | ---------------------- |
| E-mail   | `yourmail@gmail.com`   |
| Password | `PASS123456`           |

**How to use it**

1. Edit the e-mail, user name, and password in `SeedData` **before the first run** (the seed only runs while the table is empty). With Docker, the first run happens on the first `docker compose up`.
2. Log in at `POST /api/Account/login` with that e-mail and password to receive a JWT.
3. Use the token on every `🔒` endpoint.

> 🔁 To run the seed again with new credentials in Docker, reset the database with `docker compose down -v` and start again.

> ⚠️ **Security:** the default credentials are public in this repository. Always change them before exposing the API to a network, and never keep the default password in production.

## 🔐 Authentication

The admin area uses **JWT Bearer** tokens.

### `POST /api/Account/login`

```json
{
  "email": "admin@example.com",
  "password": "your-password"
}
```

Response:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

Send the token on protected requests:

```http
Authorization: Bearer <token>
```

The token carries the user's e-mail (`sub`), a unique id (`jti`), and one claim per role.

## 📄 Conventions

### Pagination

List endpoints use **`skip`** (offset) and **`take`** (page size) as route parameters:

```http
GET /api/GameModelControllers/0/50
```

| Resource                 | Max `take` |
| ------------------------ | ---------- |
| Most resources           | 50         |
| Platforms                | 150        |

Paginated responses follow this shape (the list property name varies by resource):

```json
{
  "totalGames": 120,
  "totalPages": 3,
  "currentPage": 1,
  "paginatedGames": [ { "...": "..." } ]
}
```

### Public vs. admin routes

- Routes **without** `[Authorize]` are public and read-only (plus the popularity counters).
- Management routes (`POST`, `PUT`, `DELETE`) and routes under `adm/` require a valid JWT.
- Admin list endpoints skip the cache, so changes show up immediately.

### Caching

| Data                          | Absolute expiration | Sliding expiration |
| ----------------------------- | ------------------- | ------------------ |
| Paginated lists               | 15 min              | 5 min              |
| Single items (id / slug)      | 30 min              | 10 min             |

## 📡 API Reference

Base route: `/api/[controller]`. Routes starting with `/` are absolute (not prefixed by the controller).
🔓 = public · 🔒 = requires JWT

### 🕹️ Games: `/api/GameModelControllers`

| Method   | Route                                                                  | Auth | Description                                   |
| -------- | ---------------------------------------------------------------------- | :--: | --------------------------------------------- |
| `GET`    | `/{skip}/{take}`                                                       | 🔓   | All games, sorted by name                     |
| `GET`    | `/category/{plataform}/{categoryId}/{skip}/{take}`                     | 🔓   | Games by platform and category                |
| `GET`    | `/categoryOrderByIncial/{letra}/{plataform}/{categoryId}/{skip}/{take}`| 🔓   | Same, filtered by initial letter              |
| `GET`    | `/categoryPopular/{plataform}/{categoryId}/{skip}/{take}`              | 🔓   | Popular games by platform and category        |
| `GET`    | `/categoryPopularBest/{categoryId}/{skip}/{take}`                      | 🔓   | Best-of games for a category (default take 4) |
| `GET`    | `/platform/{platformId}/{skip}/{take}`                                 | 🔓   | Games by platform                             |
| `GET`    | `/search/{query}/{skip}/{take}`                                        | 🔓   | Search by name                                |
| `GET`    | `/search/{plataform}/{query}/{skip}/{take}`                            | 🔓   | Search by name within a platform              |
| `GET`    | `/searchforplaftormandname/{plataform}/{query}/{skip}/{take}`          | 🔓   | Search by platform and name                   |
| `GET`    | `/slug/{slug}`                                                         | 🔓   | Game by slug                                  |
| `GET`    | `/platformandslug/{plataformId}/{slug}`                                | 🔓   | Game by platform id and slug                  |
| `GET`    | `/platformandslugtext/{plataform}/{slug}`                              | 🔓   | Game by platform slug and game slug          |
| `PUT`    | `/{id}/downloads`                                                      | 🔓   | Increment the game's popularity counter       |
| `GET`    | `/adm/{skip}/{take}`                                                   | 🔒   | All games (no cache)                          |
| `GET`    | `/category/adm/{plataform}/{categoryId}/{skip}/{take}`                 | 🔒   | Games by category (admin)                     |
| `GET`    | `/categoryPopular/adm/{plataform}/{categoryId}/{skip}/{take}`          | 🔒   | Popular games by category (admin)             |
| `GET`    | `/platform/adm/{platformId}/{skip}/{take}`                             | 🔒   | Games by platform (admin)                     |
| `GET`    | `/slug/adm/{slug}`                                                     | 🔒   | Game by slug (admin)                          |
| `GET`    | `/{id}`                                                                | 🔒   | Game by id                                    |
| `POST`   | `/`                                                                    | 🔒   | Create a game (validates category ids)        |
| `PUT`    | `/{id}`                                                                | 🔒   | Update a game                                 |
| `DELETE` | `/{id}`                                                                | 🔒   | Delete a game                                 |

### 🧰 Emulators: `/api/Emulator`

| Method   | Route                                        | Auth | Description                          |
| -------- | -------------------------------------------- | :--: | ------------------------------------ |
| `GET`    | `/{skip}/{take}`                             | 🔓   | All emulators                        |
| `GET`    | `/{id}`                                      | 🔓   | Emulator by id                       |
| `GET`    | `/emulatorslug/{slug}`                       | 🔓   | Emulator by slug                     |
| `GET`    | `/byname/{query}/{skip}/{take}`              | 🔓   | Search by name (absolute route)      |
| `GET`    | `/byplataform/{id}/{skip}/{take}`            | 🔓   | By platform (absolute route)         |
| `GET`    | `/adm/{skip}/{take}`                         | 🔒   | All emulators (no cache)             |
| `GET`    | `/adm/byplataform/{id}/{skip}/{take}`        | 🔒   | By platform (admin)                  |
| `GET`    | `/adm/{id}`                                  | 🔒   | Emulator by id (admin)               |
| `GET`    | `/adm/slug/{slug}`                           | 🔒   | Emulator by slug (admin)             |
| `POST`   | `/`                                          | 🔒   | Create an emulator                   |
| `PUT`    | `/{id}`                                      | 🔒   | Update an emulator                   |
| `DELETE` | `/{id}`                                      | 🔒   | Delete an emulator                   |

### 🖥️ Platforms: `/api/PlataformModels`

| Method   | Route                          | Auth | Description                         |
| -------- | ------------------------------ | :--: | ----------------------------------- |
| `GET`    | `/{skip}/{take}`               | 🔓   | All platforms (max take 150)        |
| `GET`    | `/bypopularity/{skip}/{take}`  | 🔓   | Platforms ordered by popularity     |
| `GET`    | `/Byslug/{slug}`               | 🔓   | Platform by slug                    |
| `GET`    | `/{id}`                        | 🔓   | Platform by id                      |
| `PUT`    | `/{id}/downloads`              | 🔓   | Increment the platform counter      |
| `GET`    | `/adm/{skip}/{take}`           | 🔒   | All platforms (no cache)            |
| `GET`    | `/adm/Byslug/{slug}`           | 🔒   | Platform by slug (admin)            |
| `GET`    | `/adm/{id}`                    | 🔒   | Platform by id (admin)              |
| `POST`   | `/`                            | 🔒   | Create a platform                   |
| `PUT`    | `/{id}`                        | 🔒   | Update a platform                   |
| `DELETE` | `/{id}`                        | 🔒   | Delete a platform                   |

### 🗂️ Game Categories: `/api/GameCategoryControllers`

| Method   | Route                                              | Auth | Description                         |
| -------- | -------------------------------------------------- | :--: | ----------------------------------- |
| `GET`    | `/{skip}/{take}`                                   | 🔓   | All categories                      |
| `GET`    | `/categoryByPlatform/{plataformId}/{skip}/{take}`  | 🔓   | Categories of a platform (absolute) |
| `GET`    | `/{id}`                                            | 🔓   | Category by id                      |
| `GET`    | `/adm/{skip}/{take}`                               | 🔒   | All categories (no cache)           |
| `GET`    | `/adm/categoryByPlatform/{plataformId}/{skip}/{take}` | 🔒 | Categories of a platform (admin)   |
| `GET`    | `/adm/{id}`                                        | 🔒   | Category by id (admin)              |
| `POST`   | `/`                                                | 🔒   | Create a category                   |
| `PUT`    | `/{id}`                                            | 🔒   | Update a category                   |
| `DELETE` | `/{id}`                                            | 🔒   | Delete a category                   |

### 🏷️ Game Tags: `/api/GameTagsControllers`

| Method   | Route                  | Auth | Description                          |
| -------- | ---------------------- | :--: | ------------------------------------ |
| `GET`    | `/{skip}/{take}`       | 🔓   | Tags ordered by request count        |
| `GET`    | `/slug/{slug}`         | 🔓   | Tag by slug                          |
| `PUT`    | `/{id}/downloads`      | 🔓   | Increment the tag request counter    |
| `GET`    | `/adm/{skip}/{take}`   | 🔒   | All tags (admin)                     |
| `GET`    | `/adm/{slug}`          | 🔒   | Tag by slug (admin)                  |
| `GET`    | `/{id}`                | 🔒   | Tag by id                            |
| `POST`   | `/`                    | 🔒   | Create a tag                         |
| `PUT`    | `/{id}`                | 🔒   | Update a tag                         |
| `DELETE` | `/{id}`                | 🔒   | Delete a tag                         |

### 📰 Content: Posts, Magazines, Publicity

These resources share the same CRUD pattern.

| Method   | Route                          | Auth | Description              |
| -------- | ------------------------------ | :--: | ------------------------ |
| `GET`    | `/{skip}/{take}`               | 🔓   | Paginated list           |
| `GET`    | `/{id}`                        | 🔓   | Item by id               |
| `POST`   | `/`                            | 🔒   | Create                   |
| `PUT`    | `/{id}`                        | 🔒   | Update                   |
| `DELETE` | `/{id}`                        | 🔒   | Delete                   |

| Resource       | Base route                          | Extra routes                         |
| -------------- | ----------------------------------- | ------------------------------------ |
| Posts          | `/api/Posts`                        | `GET /postsbyslug/{slug}` 🔓         |
| Magazines      | `/api/Magazine`                     | n/a                                  |
| Publicity      | `/api/PublicityControllers`         | n/a                                  |

### ☁️ Storage: `/api/Storage` (media assets)

All storage routes require a JWT. They are meant for the admin area, to manage the images and media files referenced by games, platforms, and posts.

| Method   | Route                                       | Auth | Description                                              |
| -------- | ------------------------------------------- | :--: | -------------------------------------------------------- |
| `GET`    | `/presigned?folder=&fileName=&contentType=` | 🔒   | Returns `uploadUrl`, `objectKey` and `publicUrl`         |
| `POST`   | `/upload`                                   | 🔒   | Upload a file (multipart/form-data, up to 5 GB)          |
| `POST`   | `/upload/{folder}`                          | 🔒   | Upload a file into a folder (up to 5 GB)                 |
| `GET`    | `/list`                                     | 🔒   | List all files                                           |
| `GET`    | `/list/{prefix}`                            | 🔒   | List files in a folder                                   |
| `DELETE` | `/delete/{fileName}`                        | 🔒   | Delete a file                                            |
| `DELETE` | `/delete/{pasta}/{fileName}`                | 🔒   | Delete a file from a folder                              |

#### Recommended upload flow

```
1. Admin  ──▶ GET /api/Storage/presigned?folder=covers&fileName=cover.webp
2. Admin  ──▶ PUT the image directly to `uploadUrl` (straight to the bucket)
3. Admin  ──▶ POST the returned `objectKey` / `publicUrl` in the game payload
```

Presigned URLs expire after 30 minutes.

> 💡 **Large files:** the 5 GB multipart endpoints stream the file through the API. For very large files, prefer the presigned flow above: the file goes straight to the bucket and never passes through the API server, which saves memory, bandwidth, and request time.

## 🗺️ Sitemap Endpoints (SEO)

Besides the controllers, `Program.cs` exposes **dynamic XML sitemaps** built directly from the database. All of them accept `GET` and `HEAD`, return `application/xml`, and are meant to be submitted to search engines.

| Route                                       | Type          | Description                                                                          |
| ------------------------------------------- | ------------- | ------------------------------------------------------------------------------------ |
| `/sitemap.xml`                              | Sitemap index | Lists one sitemap per platform and page (45,000 URLs per file)                       |
| `/sitemap-{platformSlug}-{page}.xml`        | URL set       | Game pages of a platform: `/games/{platformSlug}/{gameSlug}`; `404` if the page is empty |
| `/sitemap-emulators.xml`                    | URL set       | Emulator pages: `/emulators/{slug}`                                                  |
| `/sitemap-post.xml`                         | URL set       | Blog posts: `/post/{slug}`                                                           |
| `/sitemap-principais.xml`                   | URL set       | Main pages: `/games`, `/emulators`, `/post`, and `/games/{platform}/1` per platform  |

> 💡 The domains used in the sitemaps (for example `https://www.yoursite.com`) are hard-coded in `Program.cs`. Replace them with your own before deploying.

## 🧪 Usage Examples

The examples use the Docker setup (`http://localhost:7023`). Replace the host with your own when deploying elsewhere.

### List games (page 1)

```bash
curl http://localhost:7023/api/GameModelControllers/0/50
```

### Search games on a platform

```bash
curl http://localhost:7023/api/GameModelControllers/search/1/mario/0/36
```

### Increment a game's popularity counter

```bash
curl -X PUT http://localhost:7023/api/GameModelControllers/42/downloads
```

### Authenticated request

```bash
TOKEN=$(curl -s -X POST http://localhost:7023/api/Account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"your-password"}' | jq -r .token)

curl -X DELETE http://localhost:7023/api/GameModelControllers/42 \
  -H "Authorization: Bearer $TOKEN"
```

## 🗺️ Roadmap

- [ ] Apply the 5 GB limit only to the upload endpoints (`[RequestSizeLimit]`) instead of lifting the limit globally
- [ ] Add automated tests (unit with xUnit + integration with `WebApplicationFactory`)
- [ ] Add a CI pipeline (build, test, `docker compose` smoke test)
- [ ] Move pagination and filtering from in-memory LINQ to the database queries
- [ ] Move hard-coded sitemap domains and the seed user to configuration
- [ ] Refactor the duplicated sitemap code into a single service
- [ ] Assign roles (e.g. `Admin`) to the seeded user and protect admin routes by role
- [ ] Standardize response models and pagination across all controllers
- [ ] Add FluentValidation for DTOs
- [ ] Add rate limiting on public endpoints
- [ ] Add a `.dockerignore` and a non-root user to the runtime image
- [ ] Add health checks (`/health`) for the API, SQL Server, and Redis

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m "Add amazing feature"`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

Distributed under the MIT License. See `LICENSE` for details.
