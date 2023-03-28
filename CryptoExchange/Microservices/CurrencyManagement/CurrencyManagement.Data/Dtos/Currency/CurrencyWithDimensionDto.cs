using CurrencyManagement.Data.Dtos.CurrencyDimension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Dtos.Currency
{
    public class CurrencyWithDimensionDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal CurrentPriceInUSD { get; set; }

        public List<CurrencyDimensionDto>? CurrencyDimensions { get; set; }
    }
}
