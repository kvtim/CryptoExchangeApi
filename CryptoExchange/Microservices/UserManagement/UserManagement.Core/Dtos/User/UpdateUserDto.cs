using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;

namespace UserManagement.Core.Dtos.User
{
    public class UpdateUserDto
    {
        public required string? FirstName { get; set; }
        public required string? LastName { get; set; }
    }
}
