using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;

namespace UserManagement.Core.Services
{
    public interface IJWTTokenService
    {
        Task<JWTToken> CreateToken(User user);
    }
}
