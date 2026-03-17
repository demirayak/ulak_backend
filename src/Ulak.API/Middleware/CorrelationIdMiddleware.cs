using Serilog.Context;

namespace Ulak.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string HeaderName = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Header'dan gelirse onu kullan, yoksa yeni üret
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault() ?? Guid.NewGuid().ToString();

        // request içinde sakla
        context.Items["CorrelationId"] = correlationId;

        // response header'a da yaz (çok iyi pratiktir)
        context.Response.Headers[HeaderName] = correlationId;

        // Serilog context'e ekle → tüm loglara otomatik gider
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}