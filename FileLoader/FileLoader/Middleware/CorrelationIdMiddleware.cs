namespace FileLoader.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Serilog.Context;

    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderKey = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Try to get existing correlation ID, or generate a new one
            // Check for existing correlation/request ID, or generate a new one
            var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var headerVal)
                ? headerVal.ToString()
                : Guid.NewGuid().ToString();

            context.Items["CorrelationId"] = correlationId;

            // Add correlation ID to response for client tracking
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderKey] = correlationId;
                return Task.CompletedTask;
            });

            // Push correlation ID into Serilog's context
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }

}
