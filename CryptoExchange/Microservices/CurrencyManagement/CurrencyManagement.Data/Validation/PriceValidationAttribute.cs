using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyManagement.Data.Validation
{
    public class PriceValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            decimal price = (decimal)value;

            if (price <= 0)
                return new ValidationResult("Price can't be 0 or less");
            if (price > decimal.MaxValue)
                return new ValidationResult("Price is too high");

            return ValidationResult.Success;
        }
    }
}
