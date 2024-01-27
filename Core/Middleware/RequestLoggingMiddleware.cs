using System.Diagnostics;

namespace signiel.Core.Middleware;

public class RequestLoggingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        context.Response.OnStarting(() => {
            stopwatch.Stop();

            var method = context.Request.Method;
            var statusCode = context.Response.StatusCode;
            var originalUrl = context.Request.Path + context.Request.QueryString;
            var ip =
                context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ??
                context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var duration = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("{Method} {StatusCode} - {OriginalUrl} - {IP} - {UserAgent} - {Duration}ms", method, statusCode, originalUrl, ip, userAgent, duration);

            return Task.CompletedTask;
        });

        await _next(context);
    }
}
