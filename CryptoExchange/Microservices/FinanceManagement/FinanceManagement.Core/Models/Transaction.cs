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
        public int CurrencyId { get; set; }
        public int UserId { get; set; }

        public decimal CurrencyAmount { get; set; }
        public decimal PriceUSD { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
