using Logger.ElasticSearch;
using Logger.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Logger
{
    public class ElasticLogger : IElasticLogger
    {
        private readonly IElasticSearch _elasticsearch;

        public ElasticLogger(IElasticSearch elasticsearch)
        {
            _elasticsearch = elasticsearch;
            _elasticsearch.CreateIndexIfNotExists("logs");
        }

        public async Task<bool> AddOrUpdateLog(
            string microservice,
            string logType,
            string message,
            DateTime dateTime)
        {
            return await _elasticsearch.AddOrUpdate(
                new Log
                {
                    Microservice = microservice,
                    LogType = logType.ToString(),
                    Message = message,
                    LogTime = dateTime,
                });
        }

        public async Task<List<Log>> GetAllLogs()
        {
            var logs = await _elasticsearch.GetAll<Log>();

            return logs;
        }

        public async Task<List<Log>> GetLogsByMicroservice(string microservice)
        {

            var logs = await _elasticsearch.Query<Log>(
                new TermQuery()
                {
                    Field = "microservice",
                    Value = microservice
                });

            return logs;
        }

        public async Task<List<Log>> GetLogsByType(string logType)
        {
            var logs = await _elasticsearch.Query<Log>(
                new TermQuery()
                {
                    Field = "logType",
                    Value = logType
                });

            return logs;
        }

        public async Task<List<Log>> GetLogsByDate(DateTime? startDate,
            DateTime? endDate)
        {
            DateRangeQuery rangeQuery = new DateRangeQuery()
            {
                Field = "logTime",
                GreaterThanOrEqualTo = startDate,
                LessThanOrEqualTo = endDate
            };

            var logs = await _elasticsearch.Query<Log>(rangeQuery);

            return logs;
        }

        public async Task<List<Log>> SearchLogsByString(string searchString)
        {
            QueryStringQuery stringQuery = new QueryStringQuery()
            {
                DefaultField = "message",
                Query = searchString
            };

            var logs = await _elasticsearch.Query<Log>(stringQuery);

            return logs;
        }

        public async Task<long> RemoveAllLogs()
        {
            long deleted = await _elasticsearch.RemoveAll<Log>();

            return deleted;
        }
    }
}
