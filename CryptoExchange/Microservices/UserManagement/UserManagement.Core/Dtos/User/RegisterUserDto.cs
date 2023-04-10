using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Validation;

namespace UserManagement.Core.Dtos.User
{
    public class RegisterUserDto
    {
        [NameValidation]
        public required string? FirstName { get; set; }

        [NameValidation]
        public required string? LastName { get; set; }

        [EmailAddress]
        public required string? Email { get; set; }

        [UsernameValidation]
        public required string? UserName { get; set; }

        [PasswordValidation]
        public required string? Password { get; set; }
    }
}
