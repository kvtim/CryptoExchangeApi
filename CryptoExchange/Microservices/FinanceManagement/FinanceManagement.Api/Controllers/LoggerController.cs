using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Data.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly IFinanceLogger _logger;
        public LoggerController(IFinanceLogger financeLogger)
        {
            _logger = financeLogger;
        }

        [HttpGet("get-all-logs")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<Log>>> GetAllLogs()
        {
            var logsResult = await _logger.GetAllLogs();

            if (!logsResult.Succeeded)
            {
                return ApiResult.Failure(logsResult.Error);
            }
            return ApiResult.Ok(logsResult);
        }

        [HttpGet("get-logs-by-type")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<Log>>> GetLogsByType([FromQuery] string logType)
        {
            var logsResult = await _logger.GetLogsByType(logType);

            if (!logsResult.Succeeded)
            {
                return ApiResult.Failure(logsResult.Error);
            }
            return ApiResult.Ok(logsResult);
        }

        [HttpGet("get-logs-by-date")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<Log>>> GetLogsByDate(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var logsResult = await _logger.GetLogsByDate(startDate, endDate);

            if (!logsResult.Succeeded)
            {
                return ApiResult.Failure(logsResult.Error);
            }
            return ApiResult.Ok(logsResult);
        }

        [HttpGet("get-logs-by-string")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<Log>>> GetLogsByString(
            [FromQuery] string searchString)
        {
            var logsResult = await _logger.SearchLogsByString(searchString);

            if (!logsResult.Succeeded)
            {
                return ApiResult.Failure(logsResult.Error);
            }
            return ApiResult.Ok(logsResult);
        }

        [HttpDelete("remove-all-logs")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveAllLogs()
        {
            var logsResult = await _logger.RemoveAllLogs();

            if (!logsResult.Succeeded)
            {
                return ApiResult.Failure(logsResult.Error);
            }
            return ApiResult.Ok();
        }
    }
}
