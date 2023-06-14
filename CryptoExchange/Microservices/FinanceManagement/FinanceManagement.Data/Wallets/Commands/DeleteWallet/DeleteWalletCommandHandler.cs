using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Data.Logger;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, Result>
    {
        private readonly IWalletRepository _repository;
        private readonly IFinanceLogger _logger;

        public DeleteWalletCommandHandler(IWalletRepository repository,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _logger = financeLogger;
        }

        public async Task<Result> Handle(DeleteWalletCommand request, 
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                await _logger.AddOrUpdateLog(
                    LogType.Exception,
                    $"Wallet {request.Id} not found ",
                    DateTime.Now);

                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            await _repository.RemoveAsync(wallet);
            await _repository.SaveChangesAsync();

            await _logger.AddOrUpdateLog(
                LogType.Deletion,
                $"Wallet {request.Id} deleted",
                DateTime.Now);

            return Result.Ok();
        }
    }
}
