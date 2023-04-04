using FinanceManagement.Core.Dtos.Wallet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Queries.GetWalletList
{
    public class GetUserWalletsQuery : IRequest<IEnumerable<WalletDto>>
    {
        public int UserId { get; set; }
    }
}
