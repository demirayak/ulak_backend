namespace Ulak.Api.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }

    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}