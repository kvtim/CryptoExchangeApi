using FinanceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<IEnumerable<Wallet>> GetUserWalletsAsync(int userId);
        Task<Wallet> GetWalletByUserAndCurrencyAsync(int userId, int currencyId);
    }
}
