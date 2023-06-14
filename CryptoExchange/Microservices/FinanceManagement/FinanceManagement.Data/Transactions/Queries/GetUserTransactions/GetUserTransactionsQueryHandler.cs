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

namespace FinanceManagement.Data.Transactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsQueryHandler
        : IRequestHandler<GetUserTransactionsQuery, Result<IEnumerable<TransactionDto>>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFinanceLogger _logger;

        public GetUserTransactionsQueryHandler(ITransactionRepository repository, 
            IMapper mapper,
            IFinanceLogger financeLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = financeLogger;
        }

        public async Task<Result<IEnumerable<TransactionDto>>> Handle(
            GetUserTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _repository.GetUserTransactionsAsync(request.UserId);

            if (!transactions.Any())
            {
                await _logger.AddOrUpdateLog(
                   LogType.Exception,
                   $"Transactions of user {request.UserId} not found",
                   DateTime.Now);

                return Result.Failure(ErrorType.NotFound, "Transactions not found");
            }

            return Result.Ok(_mapper.Map<IEnumerable<TransactionDto>>(transactions));
        }
    }
}
