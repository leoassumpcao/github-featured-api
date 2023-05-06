namespace GithubFeatured.Middlewares
{
    public class TraceIdHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceIdHandlerMiddleware> _logger;

        public TraceIdHandlerMiddleware(RequestDelegate next, ILogger<TraceIdHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = TrySetTraceIdentifier(context);
            using (_logger.BeginScope("{@traceId}", traceId))
            {
                await _next(context);
            }
        }

        private static string TrySetTraceIdentifier(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("trace-id", out var traceId) ||
                string.IsNullOrEmpty(traceId)
            )
            {
                context.Request.Headers["trace-id"] = Guid.NewGuid().ToString();
            }

            return context.Request.Headers["trace-id"];
        }
    }
}
