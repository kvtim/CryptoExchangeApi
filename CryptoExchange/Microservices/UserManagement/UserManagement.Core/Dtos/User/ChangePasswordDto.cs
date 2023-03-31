using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Dtos.User
{
    public class ChangePasswordDto
    {
        [MinLength(6), MaxLength(127)]
        public required string? OldPassword { get; set; }
        [MinLength(6), MaxLength(127)]
        public required string NewPassword { get; set; }
    }
}
