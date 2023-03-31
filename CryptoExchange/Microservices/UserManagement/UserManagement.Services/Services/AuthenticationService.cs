using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Security;
using UserManagement.Core.Services;
using UserManagement.Core.UnitOfWork;
using UserManagement.Core.Dtos.User;

namespace UserManagement.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTTokenService _tokenService;
        private readonly IHasher _hasher;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IJWTTokenService tokenService,
            IHasher hasher
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _hasher = hasher;
        }

        public async Task<JWTToken> Authentication(LoginUserDto loginUserDto)
        {
            User user = await _unitOfWork.UserRepository.GetByUserNameAsync(loginUserDto.UserName);

            if (user == null)
                return null;

            bool confirmPassword = _hasher.Verify(loginUserDto.Password, user.Password);
            if (!confirmPassword)
                return null;

            return await _tokenService.CreateToken(user);
        }
    }
}
