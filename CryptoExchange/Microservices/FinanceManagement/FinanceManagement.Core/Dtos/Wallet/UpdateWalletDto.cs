﻿using FinanceManagement.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Dtos.Wallet
{
    public class UpdateWalletDto
    {
        [DecimalValueRange]
        public required decimal CurrencyAmount { get; set; }
    }
}
