using FinanceManagement.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
    {
        private readonly ITransactionRepository _repository;

        public DeleteTransactionCommandHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _repository.GetByIdAsync(request.Id);

            if (transaction == null)
                throw new KeyNotFoundException(nameof(transaction));

            await _repository.RemoveAsync(transaction);
            await _repository.SaveChangesAsync();
        }
    }
}
