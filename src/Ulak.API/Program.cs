using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Ulak.Api.Middleware;
using Ulak.Application;
using Ulak.Infrastructure;
using Ulak.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Serilog
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

// Layered registrations
builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
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

// Health Checks
builder.Services.AddHealthChecks();

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

// Middleware
app.UseCorrelationId();
app.UseRequestLogging();
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();