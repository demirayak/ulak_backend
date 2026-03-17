\# Ulak Backend



Ulak, modüler yapıda geliştirilen bir .NET backend projesidir.



Amaç, Clean Architecture prensiplerine uygun, genişletilebilir ve sürdürülebilir bir altyapı oluşturmaktır.



\## Kullanılan Teknolojiler



\- .NET 8

\- ASP.NET Core Web API

\- Entity Framework Core

\- PostgreSQL

\- MediatR (planlanıyor)

\- FluentValidation (planlanıyor)

\- Clean Architecture

\- CQRS (planlanıyor)



\## Katman Yapısı



```

src/



Ulak.API



Ulak.Application



Ulak.Domain



Ulak.Persistence



Ulak.Infrastructure

```



Bağımlılık kuralları:



\- API → Application

\- Application → Domain

\- Persistence → Application + Domain

\- Infrastructure → Application + Domain

\- Domain → bağımsız



\## Projeyi Çalıştırma



1\. .NET 8 kurulu olmalı

2\. PostgreSQL kurulu olmalı

3\. Migration oluşturulacak (ileride)

4\. API çalıştır:



```

dotnet run --project src/Ulak.API

```



\## Notlar



Bu proje eğitim + gerçek ürün altyapısı olarak geliştirilmektedir.

# EF Core Migration Kullanımı

## Migration Oluşturma
dotnet ef migrations add <MigrationName> -p src/Ulak.Persistence -s src/Ulak.API --output-dir Migrations/Dev

## Veritabanını Güncelleme
dotnet ef database update -p src/Ulak.Persistence -s src/Ulak.API




## Logging & Observability

This project uses **Serilog + Elasticsearch + Kibana** for centralized logging.

Logs are written to:

* Console
* File (`logs/` folder)
* Elasticsearch index (`ulak-logs-*`)

This allows monitoring requests, errors, and performance metrics in Kibana.

---

## Elasticsearch

Elasticsearch runs in Docker.

Default connection:

```
http://localhost:9200
```

Test connection:

```
http://localhost:9200
```

Expected response:

```
{
  "name": "...",
  "cluster_name": "...",
  "version": ...
}
```

---

## Kibana

Kibana UI:

```
http://localhost:5601
```

Open:

```
Discover → Create index pattern
```

Index pattern:

```
ulak-logs-*
```

Time field:

```
@timestamp
```

After that, logs will be visible.

---

## Log Fields

Each request log contains:

| Field      | Description           |
| ---------- | --------------------- |
| TraceId    | ASP.NET request id    |
| Path       | Request path          |
| Method     | HTTP method           |
| StatusCode | Response status       |
| Elapsed    | Request duration (ms) |
| IP         | Client IP             |
| User       | Authenticated user    |
| Level      | Log level             |
| Message    | Log message           |

---

## Log Files

Logs are also stored locally:

```
logs/log-YYYY-MM-DD.txt
```

Example:

```
logs/log-2026-03-17.txt
```

---

## Fail-safe logging

If Elasticsearch is down:

* Application continues running
* Logs still go to Console
* Logs still go to File

This is configured using:

```
EmitEventFailureHandling.WriteToSelfLog
FailureCallback
RequestTimeout
```

---

## Middleware logging

Request logging middleware:

```
RequestLoggingMiddleware
```

Exception logging middleware:

```
GlobalExceptionMiddleware
```

These write structured logs to Serilog.

---

## Docker containers

Check running containers:

```
docker ps
```

Expected:

* ulak_postgres
* ulak_elastic
* ulak_kibana

---

## Notes

Do not remove:

```
builder.Host.UseSerilog();
```

Without this, host-level logging will not work.


## Health Check

Uygulamanın ayakta olup olmadığını hızlıca kontrol etmek için `/health` endpoint’i eklenmiştir. Uygulama çalışırken `https://localhost:5001/health` veya ilgili host üzerinden `http://localhost:5000/health` adresi çağrılarak kontrol edilebilir. Uygulama sağlıklı şekilde çalışıyorsa bu endpoint başarılı (`200 OK`) response döner. Yapı, ileride veritabanı ve diğer bağımlılık kontrolleri eklenecek şekilde genişletilebilir olarak hazırlanmıştır.