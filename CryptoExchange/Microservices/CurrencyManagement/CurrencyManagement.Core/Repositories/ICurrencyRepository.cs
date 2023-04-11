using CurrencyManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Repositories
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Task<Currency> GetByIdWithDimensionAsync(int id);
        Task<IEnumerable<Currency>> GetAllWithDimensionAsync();
    }
}
