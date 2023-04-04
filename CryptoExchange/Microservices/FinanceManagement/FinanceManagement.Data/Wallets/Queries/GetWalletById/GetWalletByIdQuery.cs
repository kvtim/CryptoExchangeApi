using FinanceManagement.Core.Dtos.Wallet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetWalletById
{
    public class GetWalletByIdQuery : IRequest<WalletDto>
    {
        public int Id { get; set; }
    }
}
