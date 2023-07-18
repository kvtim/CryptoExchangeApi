using AutoMapper;
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
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTTokenService _tokenService;
        private readonly IHasher _hasher;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafkaProducerService;

        public RegistrationService(
            IUnitOfWork unitOfWork,
            IJWTTokenService tokenService,
            IHasher hasher,
            IMapper mapper,
            IKafkaProducerService kafkaProducerService
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _hasher = hasher;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<JWTToken>> Registration(RegisterUserDto registerUserDto)
        {
            var user = await _unitOfWork.UserRepository.GetByUserNameAsync(registerUserDto.UserName);

            if (user != null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.UserLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "User",
                        LogType = "Exception",
                        Message = $"User {registerUserDto.UserName} already exists.",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.BadRequest, "User already exists");
            }

            user = _mapper.Map<User>(registerUserDto);
            user.Password = _hasher.Hash(user.Password);

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.UserLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "User",
                        LogType = "Addition",
                        Message = $"User {user.Id} created account.",
                        LogTime = DateTime.Now
                    }));

            return Result.Ok(await _tokenService.CreateToken(user));
        }
    }
}
