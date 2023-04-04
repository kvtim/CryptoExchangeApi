using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommand : IRequest<WalletDto>
    {
        public Wallet Wallet { get; set; }
    }
}
