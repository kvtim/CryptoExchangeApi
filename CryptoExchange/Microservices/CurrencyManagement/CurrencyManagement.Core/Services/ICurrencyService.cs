using CurrencyManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Services
{
    public interface ICurrencyService : IService<Currency>
    {
        Task<IEnumerable<Currency>> GetAllWithDimensionAsync();
        Task<Currency> GetByIdlWithDimensionAsync(int id);
        Task<Currency> UpdatePriceAsync(int id, decimal increasePrice);
    }
}
