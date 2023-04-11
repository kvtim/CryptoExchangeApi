using FinanceManagement.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Dtos.Wallet
{
    public class CreateWalletDto
    {
        [IntValueRange]
        public required int CurrencyId { get; set; }

        [IntValueRange]
        public required int UserId { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        public required decimal CurrencyAmount { get; set; }
    }
}
