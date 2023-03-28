using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Dtos.Currency
{
    public class CreateCurrencyDto
    {
        public required string? Name { get; set; }
        public required decimal CurrentPriceInUSD { get; set; }
    }
}
