using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Repositories
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Wallet>> GetUserWalletsAsync(int userId)
        {
            return await _dbContext.Wallets.Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wallet> GetWalletByUserAndCurrencyAsync(int userId, int currencyId)
        {
            return await _dbContext.Wallets.FirstOrDefaultAsync(w =>
            w.UserId == userId &&
            w.CurrencyId == currencyId);
        }
    }
}
