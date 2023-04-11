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
        [NameValidation]
        public required string? FirstName { get; set; }

        [NameValidation]
        public required string? LastName { get; set; }
    }
}
