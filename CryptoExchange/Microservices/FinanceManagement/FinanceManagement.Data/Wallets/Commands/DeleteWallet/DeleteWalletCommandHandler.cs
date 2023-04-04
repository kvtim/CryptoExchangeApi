using FinanceManagement.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, Unit>
    {
        private readonly IWalletRepository _repository;

        public DeleteWalletCommandHandler(IWalletRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteWalletCommand request, 
            CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetByIdAsync(request.Id);

            if (wallet == null)
                throw new KeyNotFoundException(nameof(wallet));

            await _repository.RemoveAsync(wallet);
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
