namespace CorrelationId.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-Id";
        public const string ItemKey = "CorrelationId";

        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(correlationId))
                correlationId = Guid.NewGuid().ToString();

            context.Items[ItemKey] = correlationId;

            context.Response.Headers[HeaderName] = correlationId;

            using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
            {
                await _next(context);
            }
        }
    }

}
