using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.ErrorHandling;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsQuery : IRequest<Result<IEnumerable<TransactionDto>>>
    {
    }
}
