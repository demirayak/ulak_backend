using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Ulak.Api.Middleware;
using Ulak.Persistence.Context;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Minimal console + file + elastic (timeout) logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()

    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProcessId()

    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)

    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "ulak-logs-{0:yyyy.MM.dd}",
        NumberOfShards = 1,
        NumberOfReplicas = 0,

        FailureCallback = e => Console.WriteLine("Elastic error"),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,

        ModifyConnectionSettings = conn => conn
            .RequestTimeout(TimeSpan.FromSeconds(5))
            .DisableDirectStreaming()
    })

    .CreateLogger();

builder.Host.UseSerilog();

//DbContext
//builder.Services.AddDbContext<UlakDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
//);

builder.Services.AddDbContext<UlakDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));

    options.EnableSensitiveDataLogging(); // parametreleri görürsün
    options.EnableDetailedErrors();

    options.LogTo(log =>
    {
        Log.Information(log);
    },
    LogLevel.Information);
});

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Logging providers (opsiyonel)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ulak API",
        Version = "v1",
        Description = "Ulak backend API dokümantasyonu"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ulak API v1");
        options.RoutePrefix = "swagger";
    });
}

// Middleware sırası: Logging -> Exception -> HTTPS -> Controllers
app.UseCorrelationId();
app.UseRequestLogging();
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();