using FinanceManagement.Core.Dtos.Transaction;
using FinanceManagement.Core.ErrorHandling;
using FinanceManagement.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Result<TransactionDto>>
    {
        public Transaction Transaction { get; set; }
    }
}
