using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Dtos.User;
using UserManagement.Core.Models;
using UserManagement.Core.Security;
using UserManagement.Core.Services;
using UserManagement.Core.UnitOfWork;

namespace UserManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHasher _hasher;

        public UserService(IUnitOfWork unitOfWork, IHasher hasher)
        {
            _unitOfWork = unitOfWork;
            _hasher = hasher;
        }

        public async Task<User> AddAsync(User user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _unitOfWork.UserRepository.GetByUserNameAsync(userName);
        }

        public async Task RemoveAsync(User entity)
        {
            await _unitOfWork.UserRepository.RemoveAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<User> UpdateAsync(User entity)
        {
            await _unitOfWork.UserRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();

            return entity;
        }

        public async Task<User> ChangePassword(string userName, ChangePasswordDto changePasswordDto)
        {
            var user = await GetByUserNameAsync(userName);

            bool confirmPassword = _hasher.Verify(changePasswordDto.OldPassword, user.Password);
            if (!confirmPassword)
                return null;

            user.Password = _hasher.Hash(changePasswordDto.NewPassword);

            return await UpdateAsync(user);
        }

        public async Task<User> GetCheckedUser(
            int id,
            bool isUserAdmin,
            string userName)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (!isUserAdmin && user.UserName != userName)
            {
                throw new Exception("It isn't you");
            }

            return user;
        }

        public User SetPropertiesForUpdate(User user, UpdateUserDto userDto)
        {
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            return user;
        }
    }
}
