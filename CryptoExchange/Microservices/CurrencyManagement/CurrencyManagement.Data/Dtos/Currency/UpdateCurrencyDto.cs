using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Dtos.Currency
{
    public class UpdateCurrencyDto
    {
        public string? Name { get; set; }
        public decimal CurrentPriceInUSD { get; set; }
    }
}
