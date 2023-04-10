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
        [MinLength(2), MaxLength(50)]
        [NameValidation]
        public required string? FirstName { get; set; }

        [MinLength(2), MaxLength(100)]
        [NameValidation]
        public required string? LastName { get; set; }

        [EmailAddress]
        public required string? Email { get; set; }

        [MinLength(3), MaxLength(32)]
        [UsernameValidation]
        public required string? UserName { get; set; }

        [MinLength(6), MaxLength(127)]
        public required string? Password { get; set; }
    }
}
