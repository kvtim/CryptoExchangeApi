using EventBus.Messages.Common;
using EventBus.Messages.Events;
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

namespace FinanceManagement.Data.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, Result>
    {
        private readonly IWalletRepository _repository;
        private readonly IKafkaProducerService _kafkaProducerService;

        public DeleteWalletCommandHandler(
            IWalletRepository repository,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result> Handle(DeleteWalletCommand request,
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"Wallet {request.Id} not found ",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            await _repository.RemoveAsync(wallet);
            await _repository.SaveChangesAsync();

            await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Deletion",
                        Message = $"Wallet {request.Id} deleted",
                        LogTime = DateTime.Now
                    }));

            return Result.Ok();
        }
    }
}