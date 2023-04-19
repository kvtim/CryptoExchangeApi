using CurrencyManagement.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Dtos.Currency
{
    public class ChangeCurrencyPriceDto
    {
        [PriceValidationAttribute]
        public decimal PriceChange { get; set;}
    }
}
