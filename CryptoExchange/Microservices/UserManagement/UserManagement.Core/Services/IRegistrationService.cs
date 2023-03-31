using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Dtos.User;
using UserManagement.Core.Models;

namespace UserManagement.Core.Services
{
    public interface IRegistrationService
    {
        Task<JWTToken> Registration(RegisterUserDto registerUserDto);
    }
}
