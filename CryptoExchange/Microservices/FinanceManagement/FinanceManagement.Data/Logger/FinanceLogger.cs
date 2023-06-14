using FinanceManagement.Core.ElasticSearch;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Logger;
using FinanceManagement.Core.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = FinanceManagement.Core.ErrorHandling.Result;

namespace FinanceManagement.Data.Logger
{
    public class FinanceLogger : IFinanceLogger
    {
        private readonly IElasticSearch _elasticsearch;

        public FinanceLogger(IElasticSearch elasticsearch)
        {
            _elasticsearch = elasticsearch;
            _elasticsearch.CreateIndexIfNotExists("logs");
        }

        public async Task<bool> AddOrUpdateLog(LogType logType, string message, DateTime dateTime)
        {
            return await _elasticsearch.AddOrUpdate(
                new Log
                {
                    LogType = logType.ToString(),
                    Message = message,
                    LogTime = dateTime,
                });
        }

        public async Task<Result<List<Log>>> GetAllLogs()
        {
            var logs = await _elasticsearch.GetAll<Log>();

            if (logs == null || !logs.Any())
            {
                return Result.Failure(ErrorType.NotFound, "Logs not found");
            }

            return Result.Ok(logs);
        }

        public async Task<Result<List<Log>>> GetLogsByType(string logType)
        {
            logType = logType.ToLower();
            if (!Enum.GetValues(typeof(LogType)).Cast<LogType>()
                .ToList().Any(t => t.ToString().ToLower() == logType))
            {
                return Result.Failure(ErrorType.NotFound, "Logs not found");
            }

            var logs = await _elasticsearch.Query<Log>(
                new TermQuery()
                {
                    Field = "logType",
                    Value = logType
                });

            if (logs == null || !logs.Any())
            {
                return Result.Failure(ErrorType.NotFound, "Logs not found");
            }

            return Result.Ok(logs);
        }

        public async Task<Result<List<Log>>> GetLogsByDate(DateTime? startDate,
            DateTime? endDate)
        {
            DateRangeQuery rangeQuery = new DateRangeQuery()
            {
                Field = "logTime",
                GreaterThanOrEqualTo = startDate,
                LessThanOrEqualTo = endDate
            };

            var logs = await _elasticsearch.Query<Log>(rangeQuery);

            if (logs == null || !logs.Any())
            {
                return Result.Failure(ErrorType.NotFound, "Logs not found");
            }

            return Result.Ok(logs);
        }

        public async Task<Result<List<Log>>> SearchLogsByString(string searchString)
        {
            QueryStringQuery stringQuery = new QueryStringQuery()
            {
                DefaultField = "message",
                Query = searchString
            };

            var logs = await _elasticsearch.Query<Log>(stringQuery);

            if (logs == null || !logs.Any())
            {
                return Result.Failure(ErrorType.NotFound, "Logs not found");
            }

            return Result.Ok(logs);
        }

        public async Task<Result> RemoveAllLogs()
        {
            long deleted = await _elasticsearch.RemoveAll<Log>();

            if (deleted < 1)
            {
                return Result.Failure(ErrorType.BadRequest, "Nothing to delete");
            }

            return Result.Ok();
        }
    }
}
