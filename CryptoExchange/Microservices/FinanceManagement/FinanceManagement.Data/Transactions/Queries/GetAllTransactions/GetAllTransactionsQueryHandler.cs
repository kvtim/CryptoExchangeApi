using AutoMapper;
using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.Dtos.Wallet;
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

namespace FinanceManagement.Data.Transactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsQueryHandler
        : IRequestHandler<GetAllTransactionsQuery, Result<IEnumerable<TransactionDto>>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFinanceLogger _logger;

        public GetAllTransactionsQueryHandler(ITransactionRepository repository, IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<IEnumerable<TransactionDto>>> Handle(
            GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _repository.GetAllAsync();

            if (!transactions.Any())
            {
                await _logger.AddOrUpdateLog(
                   LogType.Exception,
                   $"Transactions not found",
                   DateTime.Now);

                return Result.Failure(ErrorType.NotFound, "Transactions not found");
            }

            return Result.Ok(_mapper.Map<IEnumerable<TransactionDto>>(transactions));
        }
    }
}
