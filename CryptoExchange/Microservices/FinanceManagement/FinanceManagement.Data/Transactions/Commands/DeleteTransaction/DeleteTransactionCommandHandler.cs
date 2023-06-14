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

namespace FinanceManagement.Data.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
    {
        private readonly ITransactionRepository _repository;
        private readonly IFinanceLogger _logger;

        public DeleteTransactionCommandHandler(ITransactionRepository repository,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _logger = financeLogger;
        }

        public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
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

            await _repository.RemoveAsync(transaction);
            await _repository.SaveChangesAsync();

            await _logger.AddOrUpdateLog(
                    LogType.Deletion,
                    $"Transaction {request.Id} deleted",
                    DateTime.Now);

            return Result.Ok();
        }
    }
}