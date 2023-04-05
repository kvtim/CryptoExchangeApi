using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Repositories
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Currency>> GetAllWithDimensionAsync()
        {
            return await _dbSet.Include(c => c.CurrencyDimensions
            .OrderByDescending(cd => cd.FromDate))
                .ToListAsync();
        }

        public async Task<Currency> GetByIdWithDimensionAsync(int id)
        {
            return await _dbSet.Include(c => c.CurrencyDimensions
            .OrderByDescending(cd => cd.FromDate))
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
