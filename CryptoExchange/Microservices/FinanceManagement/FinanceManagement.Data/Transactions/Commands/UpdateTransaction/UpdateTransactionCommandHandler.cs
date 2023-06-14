using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler
        : IRequestHandler<UpdateTransactionCommand, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFinanceLogger _logger;

        public UpdateTransactionCommandHandler(ITransactionRepository repository, IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<TransactionDto>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _repository.GetByIdAsync(request.Id);

            if (transaction == null)
            {
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"Transaction {request.Id} not found",
                    DateTime.Now);

                return Result.Failure(ErrorType.NotFound, "Transaction not found");
            }

            transaction.NewCurrencyAmount = request.NewCurrencyAmount;
            transaction.FullTransactionPriceUSD = request.FullTransactionPriceUSD;

            await _repository.UpdateAsync(transaction);
            await _repository.SaveChangesAsync();

            await _logger.AddOrUpdateLog(
                   LogType.Updation,
                   $"Transaction {request.Id} updated",
                   DateTime.Now);

            return Result.Ok(_mapper.Map<TransactionDto>(transaction));
        }
    }
}
