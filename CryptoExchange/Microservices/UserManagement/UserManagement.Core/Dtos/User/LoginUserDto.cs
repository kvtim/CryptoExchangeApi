using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Validation;

namespace UserManagement.Core.Dtos.User
{
    public class LoginUserDto
    {
        [MinLength(3), MaxLength(32)]
        [UsernameValidation]
        public required string? UserName { get; set; }

        [PasswordValidation]
        public required string? Password { get; set; }
    }
}
