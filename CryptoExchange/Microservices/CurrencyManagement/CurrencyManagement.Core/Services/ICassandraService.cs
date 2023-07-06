using CurrencyManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Services
{
    public interface ICassandraService
    {
        Task InsertAll();
        Task<List<CurrencyDimension>> GetAll();
        Task<CurrencyDimension> GetById(int id);
        Task<List<CurrencyDimension>> GetDimensionsBetweenDates(
            DateTime fromDate,
            DateTime endDate);
    }
}
