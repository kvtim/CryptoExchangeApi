using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int NewCurrencyId { get; set; }
        public int UserId { get; set; }

        public int FromCurrencyId { get; set; }
        public decimal FromCurrencyPricePerUnit { get; set; }

        public decimal NewCurrencyAmount { get; set; }
        public decimal NewCurrencyPricePerUnit { get; set; }

        public decimal FullTransactionPriceUSD { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
