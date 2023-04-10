using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Dtos.Transaction
{
    public class CreateTransactionDto
    {
        [IntValueRange]
        public int NewCurrencyId { get; set; }

        [IntValueRange]
        public int UserId { get; set; }

        [IntValueRange]
        public int FromCurrencyId { get; set; }

        [DecimalValueRange]
        public decimal FromCurrencyPricePerUnit { get; set; }

        [DecimalValueRange]
        public decimal NewCurrencyAmount { get; set; }

        [DecimalValueRange]
        public decimal NewCurrencyPricePerUnit { get; set; }
    }
}
