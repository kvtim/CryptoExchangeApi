using FinanceManagement.Core.ErrorHandling;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
