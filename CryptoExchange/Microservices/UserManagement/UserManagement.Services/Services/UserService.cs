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
using UserManagement.Core.ErrorHandling;
using MassTransit;
using EventBus.Messages.Events;

namespace UserManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHasher _hasher;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserService(
            IUnitOfWork unitOfWork,
            IHasher hasher, 
            IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _hasher = hasher;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<User> AddAsync(User user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task<Result<IEnumerable<User>>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            if (users == null)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "User",
                    LogType = "Exception",
                    Message = $"Users not found.",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.NotFound, "Users not found");
            }
            return Result.Ok(users);
        }

        public async Task<Result<User>> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "User",
                    LogType = "Exception",
                    Message = $"User {id} not found.",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.NotFound ,"User not found");
            }
            return Result.Ok(user);
        }

        public async Task<Result<User>> GetByUserNameAsync(string userName)
        {
            var user = await _unitOfWork.UserRepository.GetByUserNameAsync(userName);

            if (user == null)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "User",
                    LogType = "Exception",
                    Message = $"User {userName} not found.",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.NotFound, "User not found");
            }
            return Result.Ok(user);
        }

        public async Task RemoveAsync(User entity)
        {
            await _unitOfWork.UserRepository.RemoveAsync(entity);
            await _unitOfWork.CommitAsync();

            await _publishEndpoint.Publish(new CreateNewLogEvent()
            {
                Microservice = "User",
                LogType = "Deletion",
                Message = $"User {entity.Id} deleted.",
                LogTime = DateTime.Now
            });
        }

        public async Task<User> UpdateAsync(User entity)
        {
            await _unitOfWork.UserRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();

            await _publishEndpoint.Publish(new CreateNewLogEvent()
            {
                Microservice = "User",
                LogType = "Updation",
                Message = $"User {entity.Id} updated.",
                LogTime = DateTime.Now
            });

            return entity;
        }

        public async Task<Result<User>> ChangePassword(
            string userName,
            ChangePasswordDto changePasswordDto)
        {
            var userResult = await GetByUserNameAsync(userName);
            if (!userResult.Succeeded)
            {
                return userResult;
            }

            bool confirmPassword = _hasher.Verify(changePasswordDto.OldPassword, 
                userResult.Value.Password);
            
            if (!confirmPassword)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "User",
                    LogType = "Exception",
                    Message = $"User {userResult.Value.Id} entered wrong password.",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.BadRequest ,"Wrong old password");
            }

            userResult.Value.Password = _hasher.Hash(changePasswordDto.NewPassword);

            await _publishEndpoint.Publish(new CreateNewLogEvent()
            {
                Microservice = "User",
                LogType = "Updation",
                Message = $"User {userResult.Value.Id} changed password.",
                LogTime = DateTime.Now
            });

            return Result.Ok(await UpdateAsync(userResult.Value));
        }

        public async Task<Result<User>> GetCheckedUser(
            int id,
            bool isUserAdmin,
            string userName)
        {
            var userResult = await GetByIdAsync(id);
            if (!userResult.Succeeded)
            {
                return userResult;
            }

            if (!isUserAdmin && userResult.Value.UserName != userName)
            {
                await _publishEndpoint.Publish(new CreateNewLogEvent()
                {
                    Microservice = "User",
                    LogType = "Exception",
                    Message = $"User {userResult.Value.Id} enter wrong user.",
                    LogTime = DateTime.Now
                });

                return Result.Failure(ErrorType.BadRequest, "It isn't you");
            }

            return userResult;
        }

        public User SetPropertiesForUpdate(User user, UpdateUserDto userDto)
        {
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            return user;
        }
    }
}
