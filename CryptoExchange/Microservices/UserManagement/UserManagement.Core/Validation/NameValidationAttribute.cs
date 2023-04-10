using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserManagement.Core.Validation
{
    public class NameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string name = (string)value;

            if (!IsNameValid(name))
            {
                return new ValidationResult("Value isn't valid");
            }

            return ValidationResult.Success;
        }

        private bool IsNameValid(string name)
        {
            Regex regex = new Regex(@"^[a-zA-Z]+$");
            return regex.IsMatch(name);
        }
    }
}
