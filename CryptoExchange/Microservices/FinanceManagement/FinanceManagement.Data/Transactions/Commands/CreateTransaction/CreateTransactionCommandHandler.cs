﻿using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Logger;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Logger;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler
        : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IWalletRepository _walletRepository;
        private readonly IMapper _mapper;
        private readonly IFinanceLogger _logger;

        public CreateTransactionCommandHandler(
            ITransactionRepository repository,
            IWalletRepository walletRepository,
            IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _walletRepository = walletRepository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<TransactionDto>> Handle(
            CreateTransactionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Transaction.FromCurrencyId == request.Transaction.NewCurrencyId)
            {
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"User {request.Transaction.UserId}. " +
                    $"Sending currency and new currency are the same",
                    DateTime.Now);

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
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"User hasn't currency {request.Transaction.FromCurrencyId}",
                    DateTime.Now);

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

            await _logger.AddOrUpdateLog(
                LogType.Addition,
                $"User {request.Transaction.UserId} " +
                $"created transaction {request.Transaction.Id}. " +
                $"Bought {request.Transaction.NewCurrencyAmount} items of " +
                $"{request.Transaction.NewCurrencyId} currency " +
                $"for {request.Transaction.NewCurrencyPricePerUnit} $" +
                $"from {request.Transaction.FromCurrencyId} currency.",
                DateTime.Now);

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
                await _logger.AddOrUpdateLog(
                   LogType.Exception,
                   $"User {transaction.UserId} hasn't money for transaction",
                   DateTime.Now);

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