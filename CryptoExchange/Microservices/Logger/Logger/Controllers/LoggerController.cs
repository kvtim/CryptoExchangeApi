using Logger.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Logger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LoggerController : ControllerBase
    {
        private readonly IElasticLogger _logger;
        public LoggerController(IElasticLogger elasticLogger)
        {
            _logger = elasticLogger;
        }

        [HttpGet("get-all-logs")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logsResult = await _logger.GetAllLogs();
            
            if (logsResult == null || !logsResult.Any())
            {
                return NotFound("Logs not found");
            }

            return Ok(logsResult);
        }

        [HttpGet("get-logs-by-microservice")]
        public async Task<IActionResult> GetLogsByMicroservice([FromQuery] string microservice)
        {
            var logsResult = await _logger.GetLogsByMicroservice(microservice);

            if (logsResult == null || !logsResult.Any())
            {
                return NotFound("Logs not found");
            }

            return Ok(logsResult);
        }

        [HttpGet("get-logs-by-type")]
        public async Task<IActionResult> GetLogsByType([FromQuery] string logType)
        {
            var logsResult = await _logger.GetLogsByType(logType);

            if (logsResult == null || !logsResult.Any())
            {
                return NotFound("Logs not found");
            }

            return Ok(logsResult);
        }

        [HttpGet("get-logs-by-date")]
        public async Task<IActionResult> GetLogsByDate(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var logsResult = await _logger.GetLogsByDate(startDate, endDate);

            if (logsResult == null || !logsResult.Any())
            {
                return NotFound("Logs not found");
            }

            return Ok(logsResult);
        }

        [HttpGet("get-logs-by-string")]
        public async Task<IActionResult> GetLogsByString(
            [FromQuery] string searchString)
        {
            var logsResult = await _logger.SearchLogsByString(searchString);

            if (logsResult == null || !logsResult.Any())
            {
                return NotFound("Logs not found");
            }

            return Ok(logsResult);
        }

        [HttpDelete("remove-all-logs")]
        public async Task<IActionResult> RemoveAllLogs()
        {
            var deleted = await _logger.RemoveAllLogs();

            if (deleted < 1)
            {
                return BadRequest("Nothing to delete");
            }
            return Ok();
        }
    }
}