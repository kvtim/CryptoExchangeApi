using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Dtos.Currency
{
    public class CreateCurrencyDto
    {
        [MinLength(3), MaxLength(128)]
        public required string? Name { get; set; }

        [PriceValidationAttribute]
        public required decimal CurrentPriceInUSD { get; set; }

        [Range(1, 2)]
        public required CurrencyType CurrencyType { get; set; }
    }
}
