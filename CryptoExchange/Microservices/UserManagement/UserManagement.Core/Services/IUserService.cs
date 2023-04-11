using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Dtos.User;
using UserManagement.Core.ErrorHandling;
using UserManagement.Core.Models;

namespace UserManagement.Core.Services
{
    public interface IUserService : IService<User>
    {
        Task<Result<User>> GetByUserNameAsync(string userName);
        Task<Result<User>> ChangePassword(string userName, ChangePasswordDto changePasswordDto);
        User SetPropertiesForUpdate(User user, UpdateUserDto userDto);
        Task<Result<User>> GetCheckedUser(int id, bool isUserAdmin, string userName);
    }
}
