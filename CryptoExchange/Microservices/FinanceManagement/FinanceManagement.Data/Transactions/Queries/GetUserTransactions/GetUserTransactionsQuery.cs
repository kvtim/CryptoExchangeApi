using FinanceManagement.Core.Dtos.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public int UserId { get; set; }
    }
}
