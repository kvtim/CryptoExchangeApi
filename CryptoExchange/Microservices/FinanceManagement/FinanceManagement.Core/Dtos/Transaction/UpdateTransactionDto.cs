using FinanceManagement.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Dtos.Transaction
{
    public class UpdateTransactionDto
    {
        [DecimalValueRange]
        public decimal NewCurrencyAmount { get; set; }

        [DecimalValueRange]
        public decimal FullTransactionPriceUSD { get; set; }
    }
}
