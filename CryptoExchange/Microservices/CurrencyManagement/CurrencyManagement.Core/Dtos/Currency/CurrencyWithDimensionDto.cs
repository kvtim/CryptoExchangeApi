using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Dtos.CurrencyDimension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Core.Dtos.Currency
{
    public class CurrencyWithDimensionDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal CurrentPriceInUSD { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public List<CurrencyDimensionDto>? CurrencyDimensions { get; set; }
    }
}
