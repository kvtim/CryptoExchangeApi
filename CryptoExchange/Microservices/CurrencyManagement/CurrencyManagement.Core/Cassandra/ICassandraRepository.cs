using CurrencyManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Cassandra
{
    public interface ICassandraRepository
    {
        Task InsertList(List<CurrencyDimension> currencyDimensions);
        Task<List<CurrencyDimension>> GetAll();
        Task<CurrencyDimension> GetById(int id);
        Task<List<CurrencyDimension>> GetDimensionsBetweenDates(
            DateTime fromDate,
            DateTime endDate);
    }
}
