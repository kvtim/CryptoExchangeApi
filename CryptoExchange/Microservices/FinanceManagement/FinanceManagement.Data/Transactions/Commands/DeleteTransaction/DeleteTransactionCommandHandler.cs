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

namespace FinanceManagement.Data.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
    {
        private readonly ITransactionRepository _repository;
        private readonly IKafkaProducerService _kafkaProducerService;

        public DeleteTransactionCommandHandler(ITransactionRepository repository,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _repository.GetByIdAsync(request.Id);

            if (transaction == null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"Transaction {request.Id} not found",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.NotFound, "Transaction not found");
            }

            await _repository.RemoveAsync(transaction);
            await _repository.SaveChangesAsync();

            await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Deletion",
                        Message = $"Transaction {request.Id} deleted",
                        LogTime = DateTime.Now
                    }));

            return Result.Ok();
        }
    }
}