using AutoMapper;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.KafkaService;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Result<WalletDto>>
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafkaProducerService;

        public CreateWalletCommandHandler(
            IWalletRepository repository,
            IMapper mapper,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<WalletDto>> Handle(CreateWalletCommand request,
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetWalletByUserAndCurrencyAsync(
                request.Wallet.UserId,
                request.Wallet.CurrencyId);

            if (wallet != null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"Wallet {request.Wallet.Id}. " +
                        $"of user {request.Wallet.UserId} already exists",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.BadRequest, "Wallet already exists");
            }

            await _repository.AddAsync(request.Wallet);
            await _repository.SaveChangesAsync();

            await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Addition",
                        Message = $"Wallet {request.Wallet.Id}. " +
                        $"of user {request.Wallet.UserId} created",
                        LogTime = DateTime.Now
                    }));

            return Result.Ok(_mapper.Map<WalletDto>(request.Wallet));
        }
    }
}