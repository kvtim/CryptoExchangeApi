using CurrencyManagement.Data.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Dtos.Currency
{
    public class ChangeCurrencyPriceDto
    {
        [PriceValidationAttribute]
        public decimal PriceChange { get; set;}
    }
}
