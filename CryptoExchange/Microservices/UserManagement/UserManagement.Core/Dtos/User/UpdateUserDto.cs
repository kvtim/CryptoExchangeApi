using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;

namespace UserManagement.Core.Dtos.User
{
    public class UpdateUserDto
    {
        [MinLength(2), MaxLength(50)]
        public required string? FirstName { get; set; }

        [MinLength(2), MaxLength(100)]
        public required string? LastName { get; set; }
    }
}
