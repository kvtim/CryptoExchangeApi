using FinanceManagement.Core.Dtos.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommand : IRequest<TransactionDto>
    {
        public int Id { get; set; }

        public decimal NewCurrencyAmount { get; set; }
        public decimal FullTransactionPriceUSD { get; set; }
    }
}
