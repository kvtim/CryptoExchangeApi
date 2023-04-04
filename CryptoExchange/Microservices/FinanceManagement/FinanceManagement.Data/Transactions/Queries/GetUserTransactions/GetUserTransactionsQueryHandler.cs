using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsQueryHandler
        : IRequestHandler<GetUserTransactionsQuery, IEnumerable<TransactionDto>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public GetUserTransactionsQueryHandler(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(GetUserTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _repository.GetUserTransactionsAsync(request.UserId);

            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }
    }
}
