using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Repositories;
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

        public DeleteWalletCommandHandler(IWalletRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteWalletCommand request, 
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
            {
                return Result.Failure(ErrorType.NotFound, "Wallet not found");
            }

            await _repository.RemoveAsync(wallet);
            await _repository.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
