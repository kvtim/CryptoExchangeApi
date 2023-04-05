using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Exceptions
{
    public class CurrencyPriceLessThanZeroException : Exception
    {
        public CurrencyPriceLessThanZeroException(string currencyName)
            :base($"{currencyName} price can't be less than 0") { }
    }
}
