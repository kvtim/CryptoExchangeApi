using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler
        : IRequestHandler<CreateTransactionCommand, TransactionDto>
    {
        private readonly ITransactionRepository _repository;
        private readonly IWalletRepository _walletRepository;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHandler(
            ITransactionRepository repository,
            IWalletRepository walletRepository,
            IMapper mapper)
        {
            _repository = repository;
            _walletRepository = walletRepository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(
            CreateTransactionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Transaction.FromCurrencyId == request.Transaction.NewCurrencyId)
                throw new Exception("Sending currency and new currency are the same");

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
                throw new Exception("You haven't this currency");

            var rightNewCurrency = await _walletRepository.GetWalletByUserAndCurrencyAsync(
                request.Transaction.UserId,
                request.Transaction.NewCurrencyId);

            await UpdateUserWallet(
                request.Transaction,
                rightFromCurrency,
                rightNewCurrency,
                rightFromCurrencyAmount);


            request.Transaction.FullTransactionPriceUSD = newCurrencyPriceUSD;

            await _repository.AddAsync(request.Transaction);
            await _repository.SaveChangesAsync();

            return _mapper.Map<TransactionDto>(request.Transaction);
        }


        private decimal GetNewCurrencyPriceUSD(
            decimal newCurrencyAmount,
            decimal newCurrencyPricePerUnit) =>
            newCurrencyAmount * newCurrencyPricePerUnit;


        private decimal GetRightFromCurrencyAmount(
            decimal newCurrencyPriceUSD,
            decimal fromCurrencyPricePerUnit) =>
            newCurrencyPriceUSD / fromCurrencyPricePerUnit;


        private async Task UpdateUserWallet(
            Transaction transaction,
            Wallet rightFromUserCurrency,
            Wallet rightNewUserCurrency,
            decimal rightFromCurrencyAmount)
        {
            if (rightFromUserCurrency.CurrencyAmount <= rightFromCurrencyAmount)
                throw new Exception("You haven't money for transaction");

            rightFromUserCurrency.CurrencyAmount -= rightFromCurrencyAmount;

            if (rightNewUserCurrency != null)
            {
                rightNewUserCurrency.CurrencyAmount += transaction.NewCurrencyAmount;

                if (rightFromUserCurrency.CurrencyAmount == 0)
                    await _walletRepository.RemoveAsync(rightFromUserCurrency);
                else
                    await _walletRepository.UpdateAsync(rightFromUserCurrency);

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
        }
    }
}
