using CurrencyManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Dtos.Currency
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal CurrentPriceInUSD { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
