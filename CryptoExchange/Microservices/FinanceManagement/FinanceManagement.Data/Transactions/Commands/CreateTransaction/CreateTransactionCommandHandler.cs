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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler
        : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IWalletRepository _walletRepository;
        private readonly IMapper _mapper;
        IKafkaProducerService _kafkaProducerService;

        public CreateTransactionCommandHandler(
            ITransactionRepository repository,
            IWalletRepository walletRepository,
            IMapper mapper,
            IKafkaProducerService kafkaProducerService)
        {
            _repository = repository;
            _walletRepository = walletRepository;
            _mapper = mapper;
            _kafkaProducerService = kafkaProducerService;
        }

        public async Task<Result<TransactionDto>> Handle(
            CreateTransactionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Transaction.FromCurrencyId == request.Transaction.NewCurrencyId)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"User {request.Transaction.UserId}. " +
                        $"Sending currency and new currency are the same",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(
                    ErrorType.BadRequest,
                    "Sending currency and new currency are the same");
            }

            var newCurrencyPriceUSD = GetNewCurrencyPriceUSD(
                request.Transaction.NewCurrencyAmount,
                request.Transaction.NewCurrencyPricePerUnit);

            var rightFromCurrencyAmount = GetRightFromCurrencyAmount(
                newCurrencyPriceUSD,
                request.Transaction.FromCurrencyPricePerUnit);

            var rightFromCurrency = await _walletRepository.GetWalletByUserAndCurrencyAsync(
                request.Transaction.UserId,
                request.Transaction.FromCurrencyId);

            if (rightFromCurrency == null)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"User hasn't currency {request.Transaction.FromCurrencyId}",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.BadRequest, "You haven't this currency");
            }

            var rightNewCurrency = await _walletRepository.GetWalletByUserAndCurrencyAsync(
                request.Transaction.UserId,
                request.Transaction.NewCurrencyId);

            var updateResult = await UpdateUserWallet(
                request.Transaction,
                rightFromCurrency,
                rightNewCurrency,
                rightFromCurrencyAmount);

            if (!updateResult.Succeeded)
            {
                return updateResult;
            }


            request.Transaction.FullTransactionPriceUSD = newCurrencyPriceUSD;

            await _repository.AddAsync(request.Transaction);
            await _repository.SaveChangesAsync();

            await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Addition",
                        Message = $"User {request.Transaction.UserId} " +
                        $"created transaction {request.Transaction.Id}. " +
                        $"Bought {request.Transaction.NewCurrencyAmount} items of " +
                        $"{request.Transaction.NewCurrencyId} currency " +
                        $"for {request.Transaction.NewCurrencyPricePerUnit} $" +
                        $"from {request.Transaction.FromCurrencyId} currency.",
                        LogTime = DateTime.Now,
                    }));
           
            return Result.Ok(_mapper.Map<TransactionDto>(request.Transaction));
        }


        private decimal GetNewCurrencyPriceUSD(
            decimal newCurrencyAmount,
            decimal newCurrencyPricePerUnit) =>
            newCurrencyAmount * newCurrencyPricePerUnit;


        private decimal GetRightFromCurrencyAmount(
            decimal newCurrencyPriceUSD,
            decimal fromCurrencyPricePerUnit) =>
            newCurrencyPriceUSD / fromCurrencyPricePerUnit;


        private async Task<Result> UpdateUserWallet(
            Transaction transaction,
            Wallet rightFromUserCurrency,
            Wallet rightNewUserCurrency,
            decimal rightFromCurrencyAmount)
        {
            if (rightFromUserCurrency.CurrencyAmount <= rightFromCurrencyAmount)
            {
                await _kafkaProducerService.SendMessage(
                    TopicNamesConstants.FinanceLogsTopic,
                    JsonSerializer.Serialize(new CreateNewLogEvent()
                    {
                        Microservice = "Finance",
                        LogType = "Exception",
                        Message = $"User {transaction.UserId} hasn't money for transaction",
                        LogTime = DateTime.Now
                    }));

                return Result.Failure(ErrorType.BadRequest, "You haven't money for transaction");
            }

            rightFromUserCurrency.CurrencyAmount -= rightFromCurrencyAmount;

            if (rightNewUserCurrency != null)
            {
                rightNewUserCurrency.CurrencyAmount += transaction.NewCurrencyAmount;

                if (rightFromUserCurrency.CurrencyAmount == 0)
                {
                    await _walletRepository.RemoveAsync(rightFromUserCurrency);
                }
                else
                {
                    await _walletRepository.UpdateAsync(rightFromUserCurrency);
                }

                await _walletRepository.UpdateAsync(rightNewUserCurrency);
            }
            else
            {
                rightNewUserCurrency = new Wallet()
                {
                    UserId = transaction.UserId,
                    CurrencyId = transaction.NewCurrencyId,
                    CurrencyAmount = transaction.NewCurrencyAmount
                };

                await _walletRepository.AddAsync(rightNewUserCurrency);
            }

            return Result.Ok();
        }
    }
}