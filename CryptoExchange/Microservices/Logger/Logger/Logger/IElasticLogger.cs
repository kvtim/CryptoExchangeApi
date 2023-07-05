using Logger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Logger
{
    public interface IElasticLogger
    {
        Task<bool> AddOrUpdateLog(string microservice, string logType, string message, DateTime dateTime);
        Task<List<Log>> GetAllLogs();
        Task<List<Log>> GetLogsByMicroservice(string microservice);
        Task<List<Log>> GetLogsByType(string logType);
        Task<List<Log>> GetLogsByDate(DateTime? startDate, DateTime? endDate);
        Task<List<Log>> SearchLogsByString(string searchString);
        Task<long> RemoveAllLogs();
    }
}
