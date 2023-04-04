using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Dtos.Wallet
{
    public class CreateWalletDto
    {
        public int CurrencyId { get; set; }
        public int UserId { get; set; }

        public decimal CurrencyAmount { get; set; }
    }
}
