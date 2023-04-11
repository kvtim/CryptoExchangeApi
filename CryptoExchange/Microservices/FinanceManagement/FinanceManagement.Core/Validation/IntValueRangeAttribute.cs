using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.Validation
{
    public class IntValueRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            int price = (int)value;

            if (price <= 0)
            {
                return new ValidationResult("Value can't be 0 or less");
            }

            if (price > int.MaxValue)
            {
                return new ValidationResult("Value is too high");
            }

            return ValidationResult.Success;
        }
    }
}
