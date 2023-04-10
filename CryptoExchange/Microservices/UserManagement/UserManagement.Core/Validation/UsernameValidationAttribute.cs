using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UserManagement.Core.Validation
{
    public class UsernameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string username = (string)value;

            if (!IsUsernameValid(username))
            {
                return new ValidationResult("Username isn't valid");
            }

            return ValidationResult.Success;
        }

        private bool IsUsernameValid(string username)
        {
            Regex regex = new Regex(@"^[A-z][A-z|\.]+$");
            return regex.IsMatch(username);
        }
    }
}
