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
using UserManagement.Core.ErrorHandling;
using MassTransit;
using EventBus.Messages.Events;
using EventBus.Messages.Common;
using System.Text.Json;

namespace UserManagement.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTTokenService _tokenService;
        private readonly IHasher _hasher;
        private readonly IKafkaProducerService _kafkaProducerService;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IJWTTokenService tokenService,
            IHasher hasher,
            IKafkaProducerService kafkaProducerService
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _hasher = hasher;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<JWTToken>> Authentication(LoginUserDto loginUserDto)
        {
            User user = await _unitOfWork.UserRepository.GetByUserNameAsync(loginUserDto.UserName);

            if (user == null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.UserLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "User",
                        LogType = "Exception",
                        Message = $"Username {loginUserDto.UserName} is incorrect.",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.BadRequest, "Username is incorrect");
            }

            bool confirmPassword = _hasher.Verify(loginUserDto.Password, user.Password);
            if (!confirmPassword)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.UserLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "User",
                        LogType = "Exception",
                        Message = $"Password is incorrect.",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.BadRequest, "Password is incorrect");
            }

            return Result.Ok(await _tokenService.CreateToken(user));
        }
    }
}
