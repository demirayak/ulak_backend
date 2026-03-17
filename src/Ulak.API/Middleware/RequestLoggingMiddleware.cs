using System.Diagnostics;
using Serilog;
using Serilog.Context;

namespace Ulak.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        var traceId = context.TraceIdentifier;
        var path = context.Request.Path;
        var method = context.Request.Method;
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var user = context.User?.Identity?.Name ?? "anonymous";

        using (LogContext.PushProperty("TraceId", traceId))
        using (LogContext.PushProperty("Path", path))
        using (LogContext.PushProperty("Method", method))
        using (LogContext.PushProperty("IP", ip))
        using (LogContext.PushProperty("User", user))
        {
            try
            {
                await _next(context);

                sw.Stop();

                var statusCode = context.Response.StatusCode;

                Log.Information(
                    "HTTP {Method} {Path} responded {StatusCode} in {Elapsed} ms",
                    method,
                    path,
                    statusCode,
                    sw.ElapsedMilliseconds
                );
            }
            catch (Exception ex)
            {
                sw.Stop();

                Log.Error(
                    ex,
                    "HTTP {Method} {Path} failed in {Elapsed} ms",
                    method,
                    path,
                    sw.ElapsedMilliseconds
                );

                throw;
            }
        }
    }
}