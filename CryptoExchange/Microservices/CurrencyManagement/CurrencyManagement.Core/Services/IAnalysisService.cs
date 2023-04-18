using CurrencyManagement.Core.Dtos.Analys;
using CurrencyManagement.Core.Models;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Services
{
    public interface IAnalysisService
    {
        Task InsertDataToBigQuery();
        Task<List<CurrencyDimension>> GetAllDimensions();
        Task<DifferentBetweenDatesDto> GetDifferentBetweenFirstAndLastDate(int currencyId);
        Task<DifferentBetweenDifferentDatesDto> GetDifferentBetweenDifferentDates(
            int currencyId,
            DateTime fromDate,
            DateTime endDate);
    }
}
