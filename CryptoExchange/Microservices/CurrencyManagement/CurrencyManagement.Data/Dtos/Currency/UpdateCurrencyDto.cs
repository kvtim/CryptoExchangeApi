using CurrencyManagement.Data.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Dtos.Currency
{
    public class UpdateCurrencyDto
    {
        [MinLength(3), MaxLength(128)]
        public string? Name { get; set; }
        [PriceValidationAttribute]
        public decimal CurrentPriceInUSD { get; set; }
    }
}
