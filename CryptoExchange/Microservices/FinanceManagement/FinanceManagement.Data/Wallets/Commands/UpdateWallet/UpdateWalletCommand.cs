using FinanceManagement.Core.Dtos.Wallet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.UpdateWallet
{
    public class UpdateWalletCommand : IRequest<WalletDto>
    {
        public int Id { get; set; }
        public decimal CurrencyAmount { get; set; }
    }
}
