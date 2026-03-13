using Microsoft.EntityFrameworkCore;
using Ulak.Api.Middleware;
using Ulak.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UlakDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();