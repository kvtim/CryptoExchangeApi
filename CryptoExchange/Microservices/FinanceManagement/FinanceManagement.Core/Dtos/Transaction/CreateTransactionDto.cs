using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Dtos.Transaction
{
    public class CreateTransactionDto
    {
        public int NewCurrencyId { get; set; }
        public int UserId { get; set; }

        public int FromCurrencyId { get; set; }
        public decimal FromCurrencyPricePerUnit { get; set; }

        public decimal NewCurrencyAmount { get; set; }
        public decimal NewCurrencyPricePerUnit { get; set; }
    }
}
