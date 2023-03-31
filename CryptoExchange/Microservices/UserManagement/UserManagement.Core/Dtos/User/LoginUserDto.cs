using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Dtos.User
{
    public class LoginUserDto
    {
        [MinLength(3), MaxLength(32)]
        public required string? UserName { get; set; }

        [MinLength(6), MaxLength(127)]
        public required string? Password { get; set; }
    }
}
