using CurrencyManagement.Core.Models;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.BiqQuery
{
    public interface IBigQuery
    {
        Task<List<CurrencyDimension>> GetAllRows();
        Task<(double, double)> GetDifferentBetweenFirstAndLastDate(int currencyId);
        Task<(double, double)> GetDifferentBetweenDifferentDates(
            int currencyId,
            DateTime fromDate,
            DateTime endDate);
        Task InsertCurrencyDimensionList(List<CurrencyDimension> currencyDimensions);
        Task InsertCurrencyDimension(CurrencyDimension dimension);
        Task UpdateDimension(int id, DateTime endDate);
    }
}
