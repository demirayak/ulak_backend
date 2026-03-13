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
