using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal CurrentPriceInUSD { get; set; }

        public List<CurrencyDimension>? CurrencyDimensions { get; set; }
    }
}
