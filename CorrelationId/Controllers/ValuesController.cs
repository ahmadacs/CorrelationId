using CorrelationId.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace CorrelationId.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var correlationId = HttpContext.Items[CorrelationIdMiddleware.ItemKey];

            _logger.LogInformation("Getting values. CorrelationId = {CorrelationId}", correlationId);

            return Ok(new
            {
                Message = "Hello from Web API",
                CorrelationId = correlationId,
                Time = DateTime.UtcNow
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var correlationId = HttpContext.Items[CorrelationIdMiddleware.ItemKey];

            _logger.LogInformation("Getting value {Id}. CorrelationId = {CorrelationId}", id, correlationId);

            return Ok(new
            {
                Id = id,
                Name = $"Value {id}",
                CorrelationId = correlationId
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] string name)
        {
            var correlationId = HttpContext.Items[CorrelationIdMiddleware.ItemKey];

            _logger.LogInformation("Creating value {Name}. CorrelationId = {CorrelationId}", name, correlationId);

            return CreatedAtAction(nameof(GetById), new { id = 1 }, new
            {
                Id = 1,
                Name = name,
                CorrelationId = correlationId
            });
        }
    }
}
