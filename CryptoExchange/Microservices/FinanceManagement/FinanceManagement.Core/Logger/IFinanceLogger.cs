using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Logger
{
    public interface IFinanceLogger
    {
        Task<bool> AddOrUpdateLog(LogType logType, string message, DateTime dateTime);
        Task<Result<List<Log>>> GetAllLogs();
        Task<Result<List<Log>>> GetLogsByType(string logType);
        Task<Result<List<Log>>> GetLogsByDate(DateTime? startDate, DateTime? endDate);
        Task<Result<List<Log>>> SearchLogsByString(string searchString);
        Task<Result> RemoveAllLogs();
    }
}
