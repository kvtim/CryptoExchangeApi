using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserManagement.Core.Validation
{
    internal class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string password = (string)value;

            if (!IsPasswordValid(password))
            {
                return new ValidationResult("Value isn't valid");
            }

            return ValidationResult.Success;
        }

        private bool IsPasswordValid(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
            return regex.IsMatch(password);
        }
    }
}
