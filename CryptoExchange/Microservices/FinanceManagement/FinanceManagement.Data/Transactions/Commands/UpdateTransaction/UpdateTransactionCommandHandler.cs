using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler
        : IRequestHandler<UpdateTransactionCommand, TransactionDto>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTransactionCommandHandler(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _repository.GetByIdAsync(request.Id);

            if (transaction == null)
                throw new KeyNotFoundException(nameof(transaction));

            transaction.NewCurrencyAmount = request.NewCurrencyAmount;
            transaction.FullTransactionPriceUSD = request.FullTransactionPriceUSD;

            await _repository.UpdateAsync(transaction);
            await _repository.SaveChangesAsync();

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
