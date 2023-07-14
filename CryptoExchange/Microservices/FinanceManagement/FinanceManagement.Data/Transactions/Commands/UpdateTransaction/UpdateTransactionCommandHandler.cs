using AutoMapper;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.KafkaService;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler
        : IRequestHandler<UpdateTransactionCommand, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafkaProducerService;

        public UpdateTransactionCommandHandler(ITransactionRepository repository,
            IMapper mapper,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<TransactionDto>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
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

            transaction.NewCurrencyAmount = request.NewCurrencyAmount;
            transaction.FullTransactionPriceUSD = request.FullTransactionPriceUSD;

            await _repository.UpdateAsync(transaction);
            await _repository.SaveChangesAsync();

            await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Updation",
                        Message = $"Transaction {request.Id} updated",
                        LogTime = DateTime.Now
                    }));

            return Result.Ok(_mapper.Map<TransactionDto>(transaction));
        }
    }
}