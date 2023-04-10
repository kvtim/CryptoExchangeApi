using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Validation;

namespace UserManagement.Core.Dtos.User
{
    public class UpdateUserDto
    {
        [MinLength(2), MaxLength(50)]
        [NameValidation]
        public required string? FirstName { get; set; }

        [MinLength(2), MaxLength(100)]
        [NameValidation]
        public required string? LastName { get; set; }
    }
}
