using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.ErrorHandling;
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
        : IRequestHandler<UpdateTransactionCommand, Result<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTransactionCommandHandler(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<TransactionDto>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _repository.GetByIdAsync(request.Id);

            if (transaction == null)
            {
                return Result.Failure(ErrorType.NotFound, "Transaction not found");
            }

            transaction.NewCurrencyAmount = request.NewCurrencyAmount;
            transaction.FullTransactionPriceUSD = request.FullTransactionPriceUSD;

            await _repository.UpdateAsync(transaction);
            await _repository.SaveChangesAsync();

            return Result.Ok(_mapper.Map<TransactionDto>(transaction));
        }
    }
}
